using AutoMapper;
using BarraTour.Api.Commons;
using BarraTour.Api.Features.Users.DTOs;
using BarraTour.Api.Features.Users.Models;
using BarraTour.Api.Features.Users.Repositories;

namespace BarraTour.Api.Features.Admins.Services;

public class AdminService(IGenericRepository<User> usersRepository, IMapper mapper) : IAdminService
{
    public async Task<IEnumerable<UserResponseDto>?> GetAllUsersAsync()
    {
        // Aqui chamamos o repositório que realiza a consulta
        var users = await usersRepository.GetAllAsync();

        // Mapear para DTO
        return mapper.Map<IEnumerable<UserResponseDto>>(users);
    }
}
