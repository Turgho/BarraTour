using BarraTour.Api.Features.Users.DTOs;
using BarraTour.Api.Features.Users.Models;

namespace BarraTour.Api.Features.Admins.Services;

public interface IAdminService
{
    Task<IEnumerable<UserResponseDto>?> GetAllUsersAsync();
}