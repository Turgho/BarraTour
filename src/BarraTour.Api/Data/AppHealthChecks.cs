namespace BarraTour.Api.Data;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

public class AppHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        // Verificar se configurações essenciais estão presentes
        var jwtSecret = configuration["Jwt:SecretKey"];
        var isJwtConfigured = !string.IsNullOrEmpty(jwtSecret);

        // Verificar outras configurações importantes
        var redisConnection = configuration.GetConnectionString("Redis");
        var isRedisConfigured = !string.IsNullOrEmpty(redisConnection);

        // Coletar informações sobre o status da aplicação
        var data = new Dictionary<string, object>
        {
            { "version", GetType().Assembly.GetName().Version?.ToString() ?? "unknown" },
            { "environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development" },
            { "jwt_configured", isJwtConfigured },
            { "redis_configured", isRedisConfigured },
            { "server_time", DateTime.UtcNow }
        };

        // Determinar o status geral
        var status = HealthStatus.Healthy;
        var description = "Application is healthy";

        if (!isJwtConfigured)
        {
            status = HealthStatus.Unhealthy;
            description = "JWT configuration is missing";
        }
        else if (!isRedisConfigured)
        {
            status = HealthStatus.Degraded;
            description = "Redis configuration is missing but application can still run";
        }

        return Task.FromResult(new HealthCheckResult(
            status,
            description,
            data: data));
    }
}