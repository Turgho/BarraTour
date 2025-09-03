using BarraTour.Api.Features.Admins.Repositories;
using BarraTour.Api.Features.Companies.Repositories;
using BarraTour.Api.Features.Logs.Repositores;
using BarraTour.Api.Features.Tourists.Repositories;
using BarraTour.Api.Features.Users.Repositories;

namespace BarraTour.Api.Commons;

public interface IUnitOfWork : IDisposable
{
    // Repositórios genéricos
    IGenericRepository<T> GetRepository<T>() where T : class;
    
    // Repositórios específicos (se necessário)
    IUserRepository UserRepository { get; }
    ITouristRepository TouristRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IAdminRepository AdminRepository { get; }
    ILogRepository LogRepository { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task ExecuteWithRetryAsync(Func<Task> operation);
    Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation);
}