using BarraTour.Api.Features.Users.DTOs;

namespace BarraTour.Api.Features.Users.Services;

public interface IUserService
{
    Task<UserResponseDto?> CreateUserAsync(CreateUserRequestDto dto);
}