using BarraTour.Api.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Api.Models;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relacionamentos 1:1
    public Tourist? Tourist { get; set; }
    public Company? Company { get; set; }
    public Admin? Admin { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.UserId);

        builder.Property(u => u.UserId)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Phone)
            .HasMaxLength(20);

        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Role)
            .HasConversion<string>() // Salva como string
            .IsRequired();

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}