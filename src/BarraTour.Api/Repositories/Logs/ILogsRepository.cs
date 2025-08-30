namespace BarraTour.Api.Repositories.Logs;

public interface ILogsRepository
{
    Task LogsActionAsync(Guid userId, string action);
}