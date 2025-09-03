using BarraTour.Api.Features.Logs.Repositores;
using BarraTour.Api.Features.Users.Models;
using BarraTour.Api.Features.Users.Repositories;

namespace BarraTour.Api.Features.Auth.Repositories;

public class AuthRepository(
    ILogRepository logRepository,
    IUserRepository userRepository,
    ILogger<AuthRepository> logger)
    : IAuthRepository
{
    public string HashPassword(string password)
    {
        try
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gerar hash da senha");
            throw;
        }
    }

    public async Task<bool> VerifyPasswordAsync(User user, string password)
    {
        try
        {
            var result = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (result)
            {
                await logRepository.LogActionAsync(
                    user.UserId, 
                    "Verificação de senha bem-sucedida"
                );
            }
            else
            {
                await logRepository.LogActionAsync(
                    user.UserId, 
                    "Falha na verificação de senha"
                );
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao verificar senha para o usuário {UserId}", user.UserId);
            await logRepository.LogActionAsync(
                user.UserId, 
                $"Erro na verificação de senha: {ex.Message}"
            );
            return false;
        }
    }

    public async Task<bool> VerifyPasswordAsync(string email, string password)
    {
        try
        {
            var user = await userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                await logRepository.LogActionAsync(
                    Guid.Empty, 
                    $"Tentativa de login com email não cadastrado: {email}"
                );
                return false;
            }

            return await VerifyPasswordAsync(user, password);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao verificar senha para o email {Email}", email);
            await logRepository.LogActionAsync(
                Guid.Empty, 
                $"Erro na verificação de senha para email {email}: {ex.Message}"
            );
            return false;
        }
    }

    public async Task<bool> IsEmailRegisteredAsync(string email)
    {
        try
        {
            var user = await userRepository.GetByEmailAsync(email);
            return user != null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao verificar email cadastrado: {Email}", email);
            throw;
        }
    }

    public Task<bool> IsCpfRegisteredAsync(string cpf)
    {
        try
        {
            // Implementar lógica para verificar CPF no repositório de turistas
            // Este é um exemplo - você precisará implementar de acordo com sua estrutura
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao verificar CPF cadastrado: {Cpf}", cpf);
            throw;
        }
    }

    public Task<bool> IsCnpjRegisteredAsync(string cnpj)
    {
        try
        {
            // Implementar lógica para verificar CNPJ no repositório de empresas
            // Este é um exemplo - você precisará implementar de acordo com sua estrutura
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao verificar CNPJ cadastrado: {Cnpj}", cnpj);
            throw;
        }
    }
}