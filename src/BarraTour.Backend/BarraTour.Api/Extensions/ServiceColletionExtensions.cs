using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BarraTour.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Barra Bonita API",
                Description = "API para o sistema de reservas turísticas de Barra Bonita",
                TermsOfService = new Uri("https://barratour.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Equipe Barra Bonita",
                    Email = "suporte@barrabonita.com",
                    Url = new Uri("https://barratour.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Licença de Uso",
                    Url = new Uri("https://barratour.com/license")
                }
            });

            // Adicionar comentários XML da API
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Configurar segurança JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            // Ordenar os endpoints alfabeticamente
            c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        });

        return services;
    }
    
    public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Barra Bonita API v1");
            c.RoutePrefix = "swagger"; // Acessível em /swagger
            
            // Configurações opcionais para melhorar a UI
            c.DocumentTitle = "Barra Bonita API Documentation";
            c.DisplayRequestDuration();
            c.EnableDeepLinking();
            c.DefaultModelsExpandDepth(2);
            c.DefaultModelExpandDepth(2);
            c.DisplayOperationId();
        });

        return app;
    }

    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]
                                               ?? throw new NullReferenceException($"Chave JWT não encontrada no arquivo de configuração.")))
                };
            });

        return services;
    }
}