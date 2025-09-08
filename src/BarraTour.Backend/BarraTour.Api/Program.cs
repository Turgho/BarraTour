using BarraTour.Api.Extensions;
using BarraTour.Application.Extensions;
using BarraTour.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configurações Redis
builder.Services.AddRedisCache(builder.Configuration);

// Configurações básicas
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Camada de Application
builder.Services.AddApplicationServices();
builder.Services.AddApplicationValidators(); // Validações

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();