namespace BarraTour.Api.Services.Redis;

public interface IRedisService
{
    Task SetAsync(string key, string value, TimeSpan expiration);
    Task<string?> GetAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task RemoveAsync(string key);
}