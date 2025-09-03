using BarraTour.Api.Features.Logs.Models;

namespace BarraTour.Api.Features.Logs.Services;

public interface ILogService
{
    Task LogActionAsync(Guid userId, string action, string? entityType = null, 
        Guid? entityId = null, string? ipAddress = null, string? userAgent = null);
        
    Task LogActionAsync(HttpContext context, string action, string? entityType = null, 
        Guid? entityId = null);
        
    Task<IEnumerable<Log>> GetUserLogsAsync(Guid userId, int page = 1, int pageSize = 50);
        
    Task<IEnumerable<Log>> GetLogsByEntityAsync(string entityType, Guid entityId, 
        int page = 1, int pageSize = 50);
        
    Task<IEnumerable<Log>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate, 
        int page = 1, int pageSize = 50);
}