using BarraTour.Api.Commons;
using BarraTour.Api.Features.Tourists.Models;

namespace BarraTour.Api.Features.Tourists.Repositories;

public interface ITouristRepository : IGenericRepository<Tourist>
{
    Task<bool> CpfExistsAsync(string cpf);
}