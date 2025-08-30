using BarraTour.Api.Models;

namespace BarraTour.Api.Repositories.Tourists;

public interface ITouristRepository
{
    Task<Tourist?> GetByCpfAsync(string cpf);
    Task<Tourist?> GetByEmailAsync(string email);
    Task<Tourist?> CreateTouristAsync(Tourist tourist);
}