using BarraTour.Api.Features.Auth.DTOs;

namespace BarraTour.Api.Features.Auth.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto requestDto);
    Task LogoutAsync(string token);
}