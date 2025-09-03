using BarraTour.Api.Commons;
using BarraTour.Api.Features.Users.Models;

namespace BarraTour.Api.Features.Users.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UpdateUserLastLoginAsync(Guid userId);
}