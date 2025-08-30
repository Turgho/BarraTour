using BarraTour.Api.Data;
using BarraTour.Api.Models;
using BarraTour.Api.Repositories.Logs;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Api.Repositories.Auth;

public class AuthRepository(AppDbContext context, ILogger<AuthRepository> logger, ILogsRepository logsRepository) : IAuthRepository
{
    public async Task<User?> GetByCpfAsync(string cpf)
    {
        logger.LogInformation("Buscando turista pelo CPF: {CPF}", cpf);
        return await context.Tourists
                             .Include(t => t.User)
                             .Where(t => t.Cpf == cpf)
                             .Select(t => t.User)
                             .FirstOrDefaultAsync();
    }

    public async Task<User?> GetByCnpjAsync(string cnpj)
    {
        logger.LogInformation("Buscando empresa pelo CNPJ: {CNPJ}", cnpj);
        return await context.Companies
                             .Include(c => c.User)
                             .Where(c => c.Cnpj == cnpj)
                             .Select(c => c.User)
                             .FirstOrDefaultAsync();
    }

    public string HashPassword(string password)
    {
        logger.LogDebug("Gerando hash para a senha.");
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<bool> VerifyPasswordAsync(User user, string password)
    {
        var result = BCrypt.Net.BCrypt.Verify(password, user.Password);

        if (result)
        {
            logger.LogInformation("Senha verificada com sucesso para usuário {Email}", user.Email);
            await logsRepository.LogsActionAsync(user.UserId, "Login bem-sucedido");
        }
        else
        {
            logger.LogWarning("Falha na verificação de senha para usuário {Email}", user.Email);
            await logsRepository.LogsActionAsync(user.UserId, "Falha de login");
        }

        return result;
    }
}