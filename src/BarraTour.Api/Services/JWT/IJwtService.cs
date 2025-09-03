using System.Security.Claims;

namespace BarraTour.Api.Services.JWT;

public interface IJwtService
{
    Task<JwtValidationResult> ValidateTokenAsync(string? token);
    string GenerateToken(IEnumerable<Claim> claims, DateTime? expires = null);
}