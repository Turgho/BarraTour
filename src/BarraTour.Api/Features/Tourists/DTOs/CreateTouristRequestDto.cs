using System.ComponentModel.DataAnnotations;
using BarraTour.Api.Features.Users.DTOs;

namespace BarraTour.Api.Features.Tourists.DTOs;

public class CreateTouristRequestDto
{
    [Required]
    [StringLength(11, MinimumLength = 11)]
    public string CPF { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    [MaxLength(500)]
    public string? Preferences { get; set; }

    [MaxLength(100)]
    public string? EmergencyContactName { get; set; }

    [MaxLength(20)]
    public string? EmergencyContactPhone { get; set; }

    [Required]
    public CreateUserRequestDto User { get; set; } = null!;
}