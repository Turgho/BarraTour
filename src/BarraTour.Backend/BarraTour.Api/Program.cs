using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using BarraTour.Application.Interfaces;
using BarraTour.Application.Services;
using BarraTour.Application.Validators.User;
using BarraTour.Domain.Interfaces.Common;
using BarraTour.Domain.Interfaces.User;
using BarraTour.Infrastructure.Data;
using BarraTour.Infrastructure.Repositories.Common;
using BarraTour.Infrastructure.Repositories.User;
using BarraTour.Infrastructure.Services.Redis;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configuração básica
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException($"SQL connection string not found");

// Configuração do Redis
var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
if (!string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(sp => 
        ConnectionMultiplexer.Connect(redisConnectionString));
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
        options.InstanceName = "BarraTour";
    });
}

// Configuração do Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, 
        sqlOptions => 
        {
            sqlOptions.MigrationsAssembly("BarraTour.Api");
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});

// Configuração da Autenticação JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? throw new ArgumentNullException($"JWT SecretKey is not configured."));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configuração de Autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => 
        policy.RequireRole("admin"));
    options.AddPolicy("RequireCompanyRole", policy => 
        policy.RequireRole("company"));
    options.AddPolicy("RequireTouristRole", policy => 
        policy.RequireRole("tourist"));
});

// Configuração do Swagger com JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BarraTour API",
        Version = "v1",
        Description = "API para o sistema de turismo BarraTour",
        Contact = new OpenApiContact
        {
            Name = "Equipe BarraTour",
            Email = "contato@barraTour.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Configuração de segurança JWT 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
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
                }
            },
            Array.Empty<string>()
        }
    });

    // Usar nomes completos para evitar conflitos
    c.CustomSchemaIds(x => x.FullName);
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: sqlConnectionString,
        healthQuery: "SELECT 1;",
        name: "sqlserver",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db", "sqlserver"]
    )
    .AddRedis(
        redisConnectionString: redisConnectionString ?? throw new ArgumentNullException($"Redis connection string not found"),
        name: "redis",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["cache", "redis"]
    );

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Registro de serviços da aplicação
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRedisService, RedisService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<ITouristService, TouristService>();
// builder.Services.AddScoped<ICompanyService, CompanyService>();
// builder.Services.AddScoped<IServiceService, ServiceService>();
// builder.Services.AddScoped<IBookingService, BookingService>();

// Registro de Transaction
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Registro de repositórios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<ITouristRepository, TouristRepository>();
// builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
// builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
// builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Registro de validadores
builder.Services.AddScoped<UserValidator>();
// builder.Services.AddScoped<TouristValidator>();
// builder.Services.AddScoped<CompanyValidator>();
// builder.Services.AddScoped<ServiceValidator>();
// builder.Services.AddScoped<BookingValidator>();

// Registro de AutoMappers
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configurar pipeline de requisições
if (app.Environment.IsDevelopment())
{
    // Aplicar migrações do banco de dados em desenvolvimento
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    
    // Gera o endpoint Swagger JSON
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BarraTour API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Health Check endpoint
app.MapHealthChecks("/health");

// Rota health check
app.MapGet("/", () => new
{
    Message = "BarraTour API está funcionando!",
    Documentation = new
    {
        SwaggerUI = "/swagger",
        OpenAPI = "/swagger/v1/swagger.json"
    },
    Timestamp = DateTime.UtcNow
});

app.MapControllers();

app.Run();