using BarraTour.Api.Models;

namespace BarraTour.Api.Repositories.Auth;

public interface IAuthRepository
{
    Task<bool> VerifyPasswordAsync(User user, string password);
}