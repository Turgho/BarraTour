using System.ComponentModel.DataAnnotations;
using BarraTour.Api.Features.Users.DTOs;

namespace BarraTour.Api.Features.Companies.DTOs;

public class CreateCompanyRequestDto
{
    [Required]
    [StringLength(14, MinimumLength = 14)]
    public string CNPJ { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Address { get; set; } = null!;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(150)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Url]
    [MaxLength(255)]
    public string? LogoUrl { get; set; }

    [Required]
    public CreateUserRequestDto User { get; set; } = null!;
}