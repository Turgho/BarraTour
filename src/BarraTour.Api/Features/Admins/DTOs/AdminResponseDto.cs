using BarraTour.Api.Features.Users.DTOs;
using BarraTour.Api.Shared.Enums;

namespace BarraTour.Api.Features.Admins.DTOs;


public class AdminResponseDto
{
    public Guid AdminId { get; set; }
    public AdminLevel Level { get; set; }
    public string? Department { get; set; }
    public bool CanApproveCompanies { get; set; }
    public bool CanManageUsers { get; set; }
    public UserResponseDto? User { get; set; }
}