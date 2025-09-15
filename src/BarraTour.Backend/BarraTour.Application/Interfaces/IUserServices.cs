using BarraTour.Application.DTOs.User;
using BarraTour.Domain.Enums;

namespace BarraTour.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserResponseDto> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);
    Task<UserResponseDto> ChangeUserStatusAsync(Guid userId, UserStatus newStatus);
}