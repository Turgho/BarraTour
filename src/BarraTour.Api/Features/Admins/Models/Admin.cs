using System.ComponentModel.DataAnnotations;
using BarraTour.Api.Features.Users.Models;
using BarraTour.Api.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Api.Features.Admins.Models;

public class Admin
{
    public Guid AdminId { get; set; } = Guid.NewGuid();
        
    [Required]
    public Guid UserId { get; set; }
        
    [Required]
    [StringLength(50)]
    public AdminLevel? Level { get; set; }
        
    [StringLength(100)]
    public string? Department { get; set; }
        
    public bool CanApproveCompanies { get; set; } = true;
        
    public bool CanManageUsers { get; set; } = true;
        
    // Navigation property
    public virtual User User { get; set; } = null!;
}

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");

        builder.HasKey(a => a.AdminId);

        builder.Property(a => a.AdminId)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Level)
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(AdminLevel.Moderator);

        builder.Property(a => a.Department)
            .HasMaxLength(100);

        builder.Property(a => a.CanApproveCompanies)
            .HasDefaultValue(true);

        builder.Property(a => a.CanManageUsers)
            .HasDefaultValue(true);

        // Relacionamento com User
        builder.HasOne(a => a.User)
            .WithOne(u => u.Admin)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
