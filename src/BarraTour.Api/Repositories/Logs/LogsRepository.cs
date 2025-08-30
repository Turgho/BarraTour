using BarraTour.Api.Data;

namespace BarraTour.Api.Repositories.Logs;

public class LogsRepository(AppDbContext context, ILogger<LogsRepository> logger) : ILogsRepository
{
    public async Task LogsActionAsync(Guid userId, string action)
    {
        try
        {
            var log = new Models.Logs
            {
                UserId = userId,
                Action = action,
                CreatedAt = DateTime.UtcNow // garante hora em UTC
            };

            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gravar log para usuário {UserId}", userId);
        }
    }
}