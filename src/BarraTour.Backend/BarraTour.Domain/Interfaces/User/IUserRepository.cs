using BarraTour.Domain.Enums;
using BarraTour.Domain.Interfaces.Common;

namespace BarraTour.Domain.Interfaces.User;

public interface IUserRepository : IRepository<Domain.Entities.User>
{
    Task<Domain.Entities.User?> GetByEmailAsync(string email);
    Task<Domain.Entities.User?> GetByPhoneNumberAsync(string phoneNumber);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);
    Task<IEnumerable<Domain.Entities.User>> GetUsersByRoleAsync(UserRole role);
    Task<IEnumerable<Domain.Entities.User>> GetActiveUsersAsync();
}