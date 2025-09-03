using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BarraTour.Api.Services.Redis;
using Microsoft.IdentityModel.Tokens;

namespace BarraTour.Api.Services.JWT;

public class JwtService(IConfiguration configuration, ILogger<JwtService> logger) : IJwtService
{
        private readonly string _secretKey = 
            configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException($"SecretKey", "SecretKey não configurado");
        private readonly string _issuer = 
            configuration["Jwt:Issuer"] ?? throw new ArgumentNullException($"JWT:Issuer", "Issuer não configurado");
        private readonly string _audience = 
            configuration["Jwt:Audience"] ?? throw new ArgumentNullException($"Jwt:Audience", "Audience não configurado");

        public Task<JwtValidationResult> ValidateTokenAsync(string? token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = !string.IsNullOrEmpty(_issuer),
                    ValidIssuer = _issuer,
                    ValidateAudience = !string.IsNullOrEmpty(_audience),
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                
                return Task.FromResult(new JwtValidationResult
                {
                    IsValid = true,
                    Principal = principal
                });
            }
            catch (SecurityTokenExpiredException ex)
            {
                logger.LogWarning(ex, "Token expirado");
                return Task.FromResult(new JwtValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Token expirado"
                });
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                logger.LogWarning(ex, "Assinatura do token inválida");
                return Task.FromResult(new JwtValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Assinatura do token inválida"
                });
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Token inválido");
                return Task.FromResult(new JwtValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Token inválido"
                });
            }
        }

        public string GenerateToken(IEnumerable<Claim> claims, DateTime? expires = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires ?? DateTime.UtcNow.AddHours(2),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }