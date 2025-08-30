using BarraTour.Api.DTOs.Tourists;

namespace BarraTour.Api.Services.Users;

public interface ITouristServices
{
    Task<ReadTouristDto> CreateTouristAsync(CreateTouristDto dto);
}