using BarraTour.Api.Data;
using BarraTour.Api.DTOs.Tourists;
using BarraTour.Api.Mappers;
using BarraTour.Api.Repositories.Auth;
using BarraTour.Api.Repositories.Logs;
using BarraTour.Api.Repositories.Tourists;
using BarraTour.Api.Repositories.Users;
using BarraTour.Api.Services.Tourists;
using BarraTour.Api.Services.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar o DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Mappers
builder.Services.AddAutoMapper(typeof(TouristProfile));

// Repositórios
builder.Services.AddScoped<ILogsRepository, LogsRepository>();
builder.Services.AddScoped<ITouristRepository, TouristRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Repositórios genéricos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Serviços
builder.Services.AddScoped<ITouristServices, TouristServices>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

// Registra FluentValidation
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Registra todos os validators na assembly
builder.Services.AddValidatorsFromAssemblyContaining<CreateTouristDtoValidator>();

builder.Services.AddEndpointsApiExplorer(); // Necessário para Swagger
builder.Services.AddSwaggerGen(); // Pacote do Swagger

// Habilita CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        policy => policy
            .WithOrigins("http://localhost:4200") // URL Angular em dev
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

var app = builder.Build();

// Configurar Swagger
if (app.Environment.IsDevelopment())
{
    // Seed do banco
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.Seed(dbContext); // <— apenas aqui
    }
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Barra Bonita Turismo API v1");
    });
    
}

// Usa CORS
app.UseCors("AllowAngularDev");

// O resto do pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();