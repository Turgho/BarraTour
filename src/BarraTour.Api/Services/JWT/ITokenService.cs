namespace BarraTour.Api.Services.JWT;

public interface ITokenService
{
    Task<bool> IsTokenBlacklistedAsync(string? token);
    Task<bool> BlacklistTokenAsync(string? token, TimeSpan? expiration);
}