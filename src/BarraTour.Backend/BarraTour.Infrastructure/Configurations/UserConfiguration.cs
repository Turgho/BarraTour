using BarraTour.Domain.Entities;
using BarraTour.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarraTour.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(u => u.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.IsEmailVerified)
                .HasColumnName("is_email_verified")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(u => u.LastLogin)
                .HasColumnName("last_login")
                .IsRequired(false);

            builder.Property(u => u.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(u => u.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired(false);

            builder.Property(u => u.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            builder.Property(u => u.DeletedBy)
                .HasColumnName("deleted_by")
                .IsRequired(false);
            
            // Owned Types
            builder.OwnsOne(u => u.Name, n =>
            {
                n.Property(userName => userName.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
                n.Property(userName => userName.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
            });

            builder.OwnsOne(u => u.Email, e =>
            {
                e.Property(email => email.Value).HasColumnName("email").HasMaxLength(255).IsRequired();
                
                e.HasIndex(email => email.Value).IsUnique().HasDatabaseName("IX_Users_Email");
            });

            builder.OwnsOne(u => u.PasswordHash, p =>
            {
                p.Property(passwordHash => passwordHash.Value).HasColumnName("password_hash").HasMaxLength(100).IsRequired();
                p.Property(passwordHash => passwordHash.Salt).HasColumnName("password_salt").HasMaxLength(50).IsRequired();
                p.Property(passwordHash => passwordHash.CreatedAt).HasColumnName("password_created_at").IsRequired();
            });

            builder.OwnsOne(u => u.PhoneNumber, p =>
            {
                p.Property(phoneNumber => phoneNumber.CountryCode).HasColumnName("phone_country_code").HasMaxLength(5).IsRequired();
                p.Property(phoneNumber => phoneNumber.Number).HasColumnName("phone_number").HasMaxLength(15).IsRequired();
                
                p.HasIndex(phoneNumber => phoneNumber.Number).IsUnique().HasDatabaseName("IX_Users_PhoneNumber");
            });

            // Índices para propriedades simples
            builder.HasIndex(u => u.Role).HasDatabaseName("IX_Users_Role");
            builder.HasIndex(u => u.Status).HasDatabaseName("IX_Users_Status");
            builder.HasIndex(u => u.CreatedAt).HasDatabaseName("IX_Users_CreatedAt");
            
        // Query Filter para ignorar usuários deletados
        builder.HasQueryFilter(u => u.Status != UserStatus.Deleted);
    }
}