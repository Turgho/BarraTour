using BarraTour.Api.Shared.Enums;

namespace BarraTour.Api.DTOs.Tourists;

public class ReadTouristDto
{
    public Guid TouristId { get; set; }
    public string Cpf { get; set; } = null!;
    public string? Preferences { get; set; }

    // Informações básicas do User
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}