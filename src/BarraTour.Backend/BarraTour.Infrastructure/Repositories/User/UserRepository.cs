using BarraTour.Domain.Enums;
using BarraTour.Domain.Interfaces.User;
using BarraTour.Infrastructure.Data;
using BarraTour.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Infrastructure.Repositories.User;

public class UserRepository(AppDbContext context) : Repository<Domain.Entities.User>(context), IUserRepository
{
    public async Task<Domain.Entities.User?> GetByEmailAsync(string email)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email);
    }

    public async Task<Domain.Entities.User?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber.Number == phoneNumber);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await Context.Users
            .AnyAsync(u => u.Email.Value == email);
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
    {
        return await Context.Users
            .AnyAsync(u => u.PhoneNumber.Number == phoneNumber);
    }

    public async Task<IEnumerable<Domain.Entities.User>> GetUsersByRoleAsync(UserRole role)
    {
        return await Context.Users
            .Where(u => u.Role == role)
            .ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.User>> GetActiveUsersAsync()
    {
        return await Context.Users
            .Where(u => u.Status == UserStatus.Active)
            .ToListAsync();
    }
}