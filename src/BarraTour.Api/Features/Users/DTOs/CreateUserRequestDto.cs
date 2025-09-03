using System.ComponentModel.DataAnnotations;
using BarraTour.Api.Shared.Enums;

namespace BarraTour.Api.Features.Users.DTOs;

public class CreateUserRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = null!;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}