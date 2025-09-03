using System.ComponentModel.DataAnnotations;
using BarraTour.Api.Features.Users.DTOs;
using BarraTour.Api.Shared.Enums;

namespace BarraTour.Api.Features.Admins.DTOs;

public class CreateAdminRequestDto
{
    [Required]
    public AdminLevel Level { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    public bool CanApproveCompanies { get; set; } = true;

    public bool CanManageUsers { get; set; } = true;

    [Required]
    public CreateUserRequestDto? User { get; set; }
}