using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Api.Models;

public class Logs
{
    public long LogId { get; set; }  // BIGINT IDENTITY
    public Guid UserId { get; set; } // FK para Users
    public string Action { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamento com User
    public User User { get; set; } = null!;
}

public class LogConfiguration : IEntityTypeConfiguration<Logs>
{
    public void Configure(EntityTypeBuilder<Logs> builder)
    {
        builder.ToTable("Logs");

        builder.HasKey(l => l.LogId);

        builder.Property(l => l.LogId)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.Action)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(l => l.CreatedAt)
            .HasDefaultValueSql("SYSDATETIME()");

        // FK com User
        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}