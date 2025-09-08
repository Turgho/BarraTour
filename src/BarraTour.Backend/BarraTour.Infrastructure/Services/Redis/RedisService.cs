using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BarraTour.Infrastructure.Services.Redis;

public class RedisService(IDistributedCache cache) : IRedisService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await cache.GetStringAsync(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions();
        
        if (expiry.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiry;
        }

        var serializedValue = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, serializedValue, options);
    }

    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        var value = await cache.GetAsync(key);
        return value != null;
    }
}