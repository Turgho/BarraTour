using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using BarraTour.Infrastructure.Data;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "BarraTour API", 
        Version = "v1",
        Description = "API para sistema de turismo de Barra Bonita - SP"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
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
                }
            },
            Array.Empty<string>()
        }
    });
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
// builder.Services.AddScoped<IUserService, UserService>();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<ITouristService, TouristService>();
// builder.Services.AddScoped<ICompanyService, CompanyService>();
// builder.Services.AddScoped<IServiceService, ServiceService>();
// builder.Services.AddScoped<IBookingService, BookingService>();

// Registro de repositórios
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<ITouristRepository, TouristRepository>();
// builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
// builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
// builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Registro de validadores
// builder.Services.AddScoped<UserValidator>();
// builder.Services.AddScoped<TouristValidator>();
// builder.Services.AddScoped<CompanyValidator>();
// builder.Services.AddScoped<ServiceValidator>();
// builder.Services.AddScoped<BookingValidator>();

var app = builder.Build();

// Configurar pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BarraTour API V1");
        c.RoutePrefix = "swagger";
    });
    
    // Aplicar migrações do banco de dados em desenvolvimento
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Health Check endpoint
app.MapHealthChecks("/health");

// Rota básica para verificar se a API está funcionando
app.MapGet("/", () => Results.Ok(new { 
    message = "Barra Tour API está funcionando!", 
    timestamp = DateTime.UtcNow 
}));

app.MapControllers();

app.Run();