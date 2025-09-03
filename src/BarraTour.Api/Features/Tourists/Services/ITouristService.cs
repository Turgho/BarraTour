using BarraTour.Api.Features.Tourists.DTOs;
using BarraTour.Api.Features.Tourists.Models;

namespace BarraTour.Api.Features.Tourists.Services;

public interface ITouristService
{
    Task<TouristResponseDto?> CreateTouristAsync(CreateTouristRequestDto requestDto);
    Task<TouristResponseDto?> GetTouristByIdAsync(Guid id);
    Task<bool> CpfExistsAsync(string cpf);
}