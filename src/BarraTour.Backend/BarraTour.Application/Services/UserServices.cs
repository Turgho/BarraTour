using AutoMapper;
using BarraTour.Application.DTOs.User;
using BarraTour.Application.Interfaces;
using BarraTour.Domain.Entities;
using BarraTour.Domain.Enums;
using BarraTour.Domain.Interfaces.Common;

namespace BarraTour.Application.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var userRepository = unitOfWork.GetRepository<User>();

        // Verificar se email já existe
        if (await userRepository.ExistsAsync(u => u.Email.Value == createUserDto.Email))
        {
            throw new InvalidOperationException("Email já está em uso");
        }

        // Verificar se telefone já existe
        if (await userRepository.ExistsAsync(u => u.PhoneNumber.Number == createUserDto.PhoneNumber))
        {
            throw new InvalidOperationException("Número de telefone já está em uso");
        }

        var user = mapper.Map<User>(createUserDto);
        
        await userRepository.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid userId)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        return mapper.Map<UserResponseDto>(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var users = await userRepository.GetAllAsync();

        return mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        // Verificar se novo email já existe (se estiver sendo alterado)
        if (!string.IsNullOrEmpty(updateUserDto.Email) && 
            updateUserDto.Email != user.Email.Value &&
            await userRepository.ExistsAsync(u => u.Email.Value == updateUserDto.Email))
        {
            throw new InvalidOperationException("Email já está em uso");
        }

        mapper.Map(updateUserDto, user);
        user.UpdatedAt = DateTime.UtcNow;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<UserResponseDto>(user);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        userRepository.Remove(user);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        return await userRepository.ExistsAsync(u => u.Email.Value == email);
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        return await userRepository.ExistsAsync(u => u.PhoneNumber.Number == phoneNumber);
    }

    public async Task<UserResponseDto> ChangeUserStatusAsync(Guid userId, UserStatus newStatus)
    {
        var userRepository = unitOfWork.GetRepository<User>();
        var user = await userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        user.Status = newStatus;
        user.UpdatedAt = DateTime.UtcNow;

        if (newStatus == UserStatus.Deleted)
        {
            user.DeletedAt = DateTime.UtcNow;
        }

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<UserResponseDto>(user);
    }
}