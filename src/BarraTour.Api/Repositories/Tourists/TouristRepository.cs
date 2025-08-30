using BarraTour.Api.Data;
using BarraTour.Api.Models;
using BarraTour.Api.Repositories.Logs;
using BarraTour.Api.Repositories.Tourists;
using BarraTour.Api.Repositories.Users;
using BarraTour.Api.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace BarraTour.Api.Repositories.Tourists;

public class TouristRepository(AppDbContext context, ILogger<TouristRepository> logger, ILogsRepository logsRepository, IGenericRepository<User> userRepository) : ITouristRepository
{
    public async Task<Tourist?> GetByCpfAsync(string cpf)
    {
        logger.LogInformation("Buscando turista pelo cpf: {Cpf}", cpf);
        return await context.Tourists.FirstOrDefaultAsync(t => t.Cpf == cpf);
    }

    public async Task<Tourist?> GetByEmailAsync(string email)
    {
        logger.LogInformation("Buscando turista pelo email: {Email}", email);
        return await context.Tourists
            .Include(t => t.User) // inclui o usuário relacionado
            .FirstOrDefaultAsync(t => t.User.Email == email);
    }
    
    public async Task<Tourist?> CreateTouristAsync(Tourist tourist)
    {
        ArgumentNullException.ThrowIfNull(tourist);

        if (tourist.User == null)
            throw new ArgumentException("O turista deve ter um usuário associado.", nameof(tourist));

        // Salvar o User (caso ainda não esteja no banco)
        await userRepository.AddAsync(tourist.User);

        // Salvar o Tourist
        await context.Tourists.AddAsync(tourist);

        // Registrar log
        await logsRepository.LogsActionAsync(tourist.User.UserId, "Turista criado");
        
        await context.SaveChangesAsync(); // Garante persistência

        logger.LogInformation("Turista criado com sucesso: {Tourist}", tourist);

        return tourist;
    }
}