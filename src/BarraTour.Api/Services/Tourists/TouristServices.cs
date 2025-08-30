using AutoMapper;
using BarraTour.Api.DTOs.Tourists;
using BarraTour.Api.Models;
using BarraTour.Api.Repositories.Tourists;
using BarraTour.Api.Services.Users;
using BarraTour.Api.Shared.Enums;

namespace BarraTour.Api.Services.Tourists;

public class TouristServices(ITouristRepository touristRepository, IMapper mapper, ILogger<TouristServices> logger) : ITouristServices
{
    public async Task<ReadTouristDto> CreateTouristAsync(CreateTouristDto dto)
    {
        // Criar User
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Tourist,
            Status = UserStatus.Active
        };

        // Criar Tourist associado ao User
        var tourist = new Tourist
        {
            User = user,
            Cpf = dto.Cpf
        };

        // 3️⃣ Persistir via TouristRepository
        await touristRepository.CreateTouristAsync(tourist);

        logger.LogInformation("Turista criado com sucesso: {Tourist}", tourist);

        // 4️⃣ Mapear para DTO de leitura
        return mapper.Map<ReadTouristDto>(tourist);
    }
}
