using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Api.Models;

public class Admin
{
    public Guid AdminId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Level { get; set; } = "standard";

    // Relacionamento
    public User User { get; set; } = null!;
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
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("standard");

        builder.HasOne(a => a.User)
            .WithOne(u => u.Admin)
            .HasForeignKey<Admin>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
