using BarraTour.Api.Features.Users.Models;

namespace BarraTour.Api.Features.Companies.Repositories;

public interface ICompanyRepository
{
    Task<User?> GetByCnpjAsync(string cnpj);
}