using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BarraTour.Api.Commons;
using BarraTour.Api.Features.Admins.DTOs;
using BarraTour.Api.Features.Admins.Models;
using BarraTour.Api.Features.Auth.DTOs;
using BarraTour.Api.Features.Auth.Repositories;
using BarraTour.Api.Features.Companies.DTOs;
using BarraTour.Api.Features.Companies.Models;
using BarraTour.Api.Features.Tourists.DTOs;
using BarraTour.Api.Features.Tourists.Models;
using BarraTour.Api.Features.Users.Models;
using BarraTour.Api.Features.Users.Repositories;
using BarraTour.Api.Services.JWT;
using BarraTour.Api.Services.Redis;
using BarraTour.Api.Shared.Enums;
using Microsoft.IdentityModel.Tokens;

namespace BarraTour.Api.Features.Auth.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    ILogger<AuthService> logger,
    IConfiguration configuration,
    IAuthRepository authRepository,
    IUserRepository userRepository,
    IRedisService redisService,
    ITokenService tokenService,
    IMapper mapper) : IAuthService
{
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto requestDto)
    {
        try
        {
            // Validação básica do DTO
            if (string.IsNullOrWhiteSpace(requestDto.Email) || string.IsNullOrWhiteSpace(requestDto.Password))
            {
                logger.LogWarning("Tentativa de login com credenciais vazias");
                return null;
            }

            var user = await userRepository.GetByEmailAsync(requestDto.Email);
            if (user == null)
            {
                logger.LogWarning("Tentativa de login com email não encontrado: {Email}", requestDto.Email);
                return null;
            }

            // Verifica se o usuário está ativo
            if (user.Status != UserStatus.Active)
            {
                logger.LogWarning("Tentativa de login com usuário inativo: {Email}", requestDto.Email);
                return null;
            }

            // Verificação de senha
            var valid = await authRepository.VerifyPasswordAsync(user, requestDto.Password);
            if (!valid)
            {
                logger.LogWarning("Tentativa de login com senha inválida para: {Email}", requestDto.Email);
                return null;
            }

            // Verifica Redis para token existente
            var cachedToken = await redisService.GetAsync($"jwt:{user.UserId}");
            
            // Verificar se o token em cache não está na blacklist
            if (!string.IsNullOrEmpty(cachedToken) && !await tokenService.IsTokenBlacklistedAsync(cachedToken))
            {
                logger.LogInformation("Token reutilizado do cache para o usuário {UserId}", user.UserId);
                return await BuildLoginResponseAsync(user, cachedToken);
            }

            // Gera novo token
            var tokenString = await GenerateJwtTokenAsync(user);
            
            // Cache do novo token com tempo de expiração
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(tokenString);
            var expiration = jwtToken.ValidTo - DateTime.UtcNow;
            
            if (expiration > TimeSpan.Zero)
            {
                await redisService.SetAsync($"jwt:{user.UserId}", tokenString, expiration);
            }
            
            // Atualiza último login
            await unitOfWork.UserRepository.UpdateUserLastLoginAsync(user.UserId);
            
            // Prepara resposta
            var response = await BuildLoginResponseAsync(user, tokenString);
            
            logger.LogInformation("Login bem-sucedido para o usuário {UserId}", user.UserId);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante o login para o email: {Email}", requestDto.Email);
            throw;
        }
    }

    public async Task LogoutAsync(string token)
    {
        try
        {
            // Adiciona token à blacklist
            await tokenService.BlacklistTokenAsync(token, TimeSpan.FromHours(24));
            
            // Remove token do cache
            var userId = GetUserIdFromToken(token);
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var userGuid))
            {
                await redisService.RemoveAsync($"jwt:{userGuid}");
            }
            
            logger.LogInformation("Logout realizado com sucesso");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante o logout");
            throw;
        }
    }
    
    // ====== funções auxiliares ======

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!);
            
            // Obter claims do usuário
            var claims = await GetUserClaimsAsync(user);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(configuration["Jwt:ExpireHours"] ?? "2")),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gerar token JWT para o usuário {UserId}", user.UserId);
            throw;
        }
    }

    private async Task<List<Claim>> GetUserClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        
        // Adicionar claims específicas baseadas no tipo de usuário
        switch (user.Role)
        {
            case UserRole.Tourist:
                var tourist = await unitOfWork.GetRepository<Tourist>().GetByIdAsync(user.UserId);
                if (tourist != null)
                {
                    claims.Add(new Claim("TouristId", tourist.TouristId.ToString()));
                }
                break;
                
            case UserRole.Company:
                var company = await unitOfWork.GetRepository<Company>().GetByIdAsync(user.UserId);
                if (company != null)
                {
                    claims.Add(new Claim("CompanyId", company.CompanyId.ToString()));
                }
                break;
                
            case UserRole.Admin:
                var admin = await unitOfWork.GetRepository<Admin>().GetByIdAsync(user.UserId);
                if (admin != null)
                {
                    claims.Add(new Claim("AdminId", admin.AdminId.ToString()));
                }
                break;
        }
        
        return claims;
    }

    private Task<LoginResponseDto> BuildLoginResponseAsync(User user, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var expiresAt = jwtToken.ValidTo;
        
        var response = new LoginResponseDto
        {
            Token = token,
            UserId = user.UserId,
            ExpiresAt = expiresAt
        };
        
        // Preencher dados específicos baseados no tipo de usuário
        // Agora as entidades relacionadas já estão carregadas
        switch (user.Role)
        {
            case UserRole.Tourist:
                if (user.Tourist != null)
                {
                    response.Tourist = mapper.Map<TouristResponseDto>(user.Tourist);
                }
                break;
            
            case UserRole.Company:
                if (user.Company != null)
                {
                    response.Company = mapper.Map<CompanyResponseDto>(user.Company);
                }
                break;
            
            case UserRole.Admin:
                if (user.Admin != null)
                {
                    response.Admin = mapper.Map<AdminResponseDto>(user.Admin);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return Task.FromResult(response);
    }

    private string? GetUserIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token.Replace("Bearer ", ""));
            
            // Tentar obter o userId de diferentes claims possíveis
            var userId = jwtToken.Claims.FirstOrDefault(c => 
                c.Type == JwtRegisteredClaimNames.Sub || 
                c.Type == "userId" || 
                c.Type == ClaimTypes.NameIdentifier)?.Value;
                
            return userId;
        }
        catch
        {
            return null;
        }
    }
}