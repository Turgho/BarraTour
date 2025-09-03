using BarraTour.Api.Data;
using BarraTour.Api.Features.Auth.Repositories;
using BarraTour.Api.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Api.Features.Companies.Repositories;

public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
    public async Task<User?> GetByCnpjAsync(string cnpj)
    {
        return await context.Companies
            .Include(c => c.User)
            .Where(c => c.CNPJ == cnpj)
            .Select(c => c.User)
            .FirstOrDefaultAsync();
    }
}