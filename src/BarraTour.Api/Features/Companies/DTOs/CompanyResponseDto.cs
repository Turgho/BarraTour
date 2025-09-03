using BarraTour.Api.Features.Users.DTOs;
using BarraTour.Api.Shared.Enums;

namespace BarraTour.Api.Features.Companies.DTOs;

public class CompanyResponseDto
{
    public Guid CompanyId { get; set; }
    public string CNPJ { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public UserResponseDto User { get; set; } = null!;
}
