using BarraTour.Api.Features.Users.Models;

namespace BarraTour.Api.Features.Auth.Repositories;

public interface IAuthRepository
{
    string HashPassword(string password);
    Task<bool> VerifyPasswordAsync(User user, string password);
    Task<bool> VerifyPasswordAsync(string email, string password);
    Task<bool> IsEmailRegisteredAsync(string email);
    Task<bool> IsCpfRegisteredAsync(string cpf);
    Task<bool> IsCnpjRegisteredAsync(string cnpj);
}