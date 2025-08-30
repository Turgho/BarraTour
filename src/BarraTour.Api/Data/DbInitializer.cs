using BarraTour.Api.Models;
using BarraTour.Api.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Api.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        context.Database.Migrate(); // aplica todas as migrations pendentes

        if (context.Users.Any())
            return; // Já populado

        // Usuários iniciais
        var users = new List<User>
        {
            new User { Name = "Admin Test", Email = "admin@barratour.com", Password = "admin123", Phone = "11999999999", Role = UserRole.Admin, CreatedAt = DateTime.UtcNow },
            new User { Name = "Tourist 1", Email = "tourist1@barratour.com", Password = "tourist123", Phone = "11988888801", Role = UserRole.Tourist, CreatedAt = DateTime.UtcNow },
            new User { Name = "Tourist 2", Email = "tourist2@barratour.com", Password = "tourist123", Phone = "11988888802", Role = UserRole.Tourist, CreatedAt = DateTime.UtcNow },
            new User { Name = "Tourist 3", Email = "tourist3@barratour.com", Password = "tourist123", Phone = "11988888803", Role = UserRole.Tourist, CreatedAt = DateTime.UtcNow },
            new User { Name = "Tourist 4", Email = "tourist4@barratour.com", Password = "tourist123", Phone = "11988888804", Role = UserRole.Tourist, CreatedAt = DateTime.UtcNow },
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        // Turistas com CPFs válidos
        var tourists = new List<Tourist>
        {
            new Tourist { UserId = users[1].UserId, Cpf = "14059477052", Preferences = "Praias, Museus" },
            new Tourist { UserId = users[2].UserId, Cpf = "71682963020", Preferences = "Parques, Restaurantes" },
            new Tourist { UserId = users[3].UserId, Cpf = "44389309013", Preferences = "Museus, Restaurantes" },
            new Tourist { UserId = users[4].UserId, Cpf = "49428767026", Preferences = "Parques, Praias" },
        };
        
        context.Tourists.AddRange(tourists);
        context.SaveChanges();

        // Logs
        var logs = users.SelectMany(u => new List<Logs>
        {
            new Logs { UserId = u.UserId, Action = "Login", CreatedAt = DateTime.UtcNow },
            new Logs { UserId = u.UserId, Action = "Visualizou perfil", CreatedAt = DateTime.UtcNow }
        }).ToList();

        context.Logs.AddRange(logs);
        context.SaveChanges();
    }
}