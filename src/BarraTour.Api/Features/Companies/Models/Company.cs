using System.ComponentModel.DataAnnotations;
using BarraTour.Api.Features.Users.Models;
using BarraTour.Api.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Api.Features.Companies.Models;

public class Company
{
    public Guid CompanyId { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    [StringLength(14)]
    public string? CNPJ { get; set; } = null!;
    
    [Required]
    [StringLength(150)]
    public string? Name { get; set; } = null!;
    
    [Required]
    [StringLength(255)]
    public string? Address { get; set; } = null!;
    
    [StringLength(20)]
    public string? Phone { get; set; }
    
    [StringLength(150)]
    public string? Email { get; set; }
    
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
    
    public Guid? ApprovedBy { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    [StringLength(500)]
    public string? RejectionReason { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(255)]
    public string? LogoUrl { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual User? Approver { get; set; }
}

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.CompanyId);

        builder.Property(c => c.CompanyId)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.CNPJ)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(c => c.CNPJ)
            .IsUnique();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Phone)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(150);

        builder.Property(c => c.ApprovalStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ApprovalStatus.Pending);

        builder.Property(c => c.RejectionReason)
            .HasMaxLength(500);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.LogoUrl)
            .HasMaxLength(255);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("SYSDATETIME()");

        // Relacionamento com User
        builder.HasOne(c => c.User)
            .WithOne(u => u.Company)
            .HasForeignKey<Company>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento com Admin (ApprovedBy)
        builder.HasOne(c => c.Approver)
            .WithMany()
            .HasForeignKey(c => c.ApprovedBy)
            .OnDelete(DeleteBehavior.NoAction);
    }
}