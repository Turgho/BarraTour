using BarraTour.Api.Extensions;
using BarraTour.Application.Extensions;
using BarraTour.Infrastructure.Extensions;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Data Protection PRIMEIRO (antes de outros serviços)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"))
        .SetApplicationName("BarraTourApp")
        .UseEphemeralDataProtectionProvider();
}

// Configurações Redis
builder.Services.AddRedisCache(builder.Configuration);

// Configurações básicas
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Camada de Application
builder.Services.AddApplicationServices();
builder.Services.AddApplicationValidators();

// Camada de Infrastructure
builder.Services.AddInfrastructureDbContext(builder.Configuration);
builder.Services.AddInfrastructureRepositories();

// Camada de API
builder.Services.AddApiDocumentation();
builder.Services.AddApiAuthentication(builder.Configuration);

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseApiDocumentation();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();