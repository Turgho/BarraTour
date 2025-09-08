using System.Reflection;
using BarraTour.Infrastructure.Data;
using BarraTour.Infrastructure.Services.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarraTour.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
    
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        // Criar instância e fazer binding manualmente
        var redisConfig = new RedisConfiguration();
        configuration.GetSection("Redis").Bind(redisConfig);
    
        var redisOptions = configuration.GetSection("Redis").Get<RedisConfiguration>();
    
        services.AddStackExchangeRedisCache(options =>
        {
            if (redisOptions == null) return;
            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = redisOptions.InstanceName;
        });

        services.AddScoped<IRedisService, RedisService>();

        return services;
    }
}