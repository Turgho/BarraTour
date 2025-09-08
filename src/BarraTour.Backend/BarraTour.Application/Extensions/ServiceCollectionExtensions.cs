using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BarraTour.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar todos os serviços da camada Application
        var assembly = Assembly.GetExecutingAssembly();
        
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        // Registrar validadores FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}