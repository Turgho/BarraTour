using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Api.Models;

public class Tourist
{
    public Guid TouristId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Cpf { get; set; } = null!;
    public string? Preferences { get; set; }

    // Relacionamento
    public User User { get; set; } = null!;
}

public class TouristConfiguration : IEntityTypeConfiguration<Tourist>
{
    public void Configure(EntityTypeBuilder<Tourist> builder)
    {
        builder.ToTable("Tourists");

        builder.HasKey(t => t.TouristId);

        builder.Property(t => t.TouristId)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Cpf)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasIndex(t => t.Cpf)
            .IsUnique();

        builder.Property(t => t.Preferences)
            .HasMaxLength(500);

        builder.HasOne(t => t.User)
            .WithOne(u => u.Tourist)
            .HasForeignKey<Tourist>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}