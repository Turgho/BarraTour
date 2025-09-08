namespace BarraTour.Infrastructure.Services.Redis;

public class RedisConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = "BarraTour";
    public int DefaultExpirationMinutes { get; set; } = 60;
}