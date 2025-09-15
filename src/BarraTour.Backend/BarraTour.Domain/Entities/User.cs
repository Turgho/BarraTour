using BarraTour.Domain.Enums;
using BarraTour.Domain.ValueObjects.User;

namespace BarraTour.Domain.Entities;

public class User
{
    public Guid UserId { get; init; } = Guid.NewGuid();
    public required UserName Name { get; set; }
    public required Email Email { get; set; }
    public required PasswordHash PasswordHash { get; set; }
    public required PhoneNumber PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Tourist;
    public UserStatus Status { get; set; } = UserStatus.PendingVerification;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}