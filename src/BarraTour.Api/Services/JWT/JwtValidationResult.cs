using System.Security.Claims;

namespace BarraTour.Api.Services.JWT;

public class JwtValidationResult
{
    public bool IsValid { get; set; }
    public ClaimsPrincipal? Principal { get; set; }
    public string? ErrorMessage { get; set; }
}