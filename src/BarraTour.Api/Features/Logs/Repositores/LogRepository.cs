using BarraTour.Api.Data;
using BarraTour.Api.Features.Logs.Models;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Api.Features.Logs.Repositores;

public class LogRepository(AppDbContext context, ILogger<LogRepository> logger) : ILogRepository
{
    public async Task LogActionAsync(Guid userId, string action, string? entityType,
        Guid? entityId, string? ipAddress, string? userAgent)
    {
        try
        {
            // Verificar se o userId é válido
            if (userId == Guid.Empty)
            {
                logger.LogWarning("Tentativa de registrar log com UserId vazio");
                return;
            }

            // Verificar se o usuário existe no banco de dados
            var userExists = await context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                logger.LogWarning("Tentativa de registrar log para usuário não existente: {UserId}", userId);
                return;
            }

            var log = new Log
            {
                LogId = Guid.NewGuid(),
                UserId = userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                CreatedAt = DateTime.UtcNow
            };

            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao gravar log para usuário {UserId}", userId);
            // Não relançar a exceção para não interromper o fluxo principal
        }
    }

    public async Task<IEnumerable<Log>> GetUserLogsAsync(Guid userId, int page = 1, int pageSize = 50)
    {
        return await context.Logs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Log>> GetLogsByEntityAsync(string entityType, Guid entityId, 
        int page = 1, int pageSize = 50)
    {
        return await context.Logs
            .Where(l => l.EntityType == entityType && l.EntityId == entityId)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, 
        int page = 1, int pageSize = 50)
    {
        return await context.Logs
            .Where(l => l.CreatedAt >= startDate && l.CreatedAt <= endDate)
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}