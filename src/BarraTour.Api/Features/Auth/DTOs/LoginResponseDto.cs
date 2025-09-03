using BarraTour.Api.Features.Admins.DTOs;
using BarraTour.Api.Features.Companies.DTOs;
using BarraTour.Api.Features.Tourists.DTOs;

namespace BarraTour.Api.Features.Auth.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    // Propriedades opcionais para dados específicos do perfil
    public TouristResponseDto? Tourist { get; set; }
    public CompanyResponseDto? Company { get; set; }
    public AdminResponseDto? Admin { get; set; }
}
