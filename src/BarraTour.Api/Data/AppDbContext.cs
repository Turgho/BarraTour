using BarraTour.Api.Features.Admins.Models;
using BarraTour.Api.Features.Companies.Models;
using BarraTour.Api.Features.Logs.Models;
using BarraTour.Api.Features.Tourists.Models;
using BarraTour.Api.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // DbSets
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tourist> Tourists { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Log> Logs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica cada configuração separada
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TouristConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new LogConfiguration());
    }
}