using System.ComponentModel.DataAnnotations;
using BarraTour.Domain.Enums;

namespace BarraTour.Application.DTOs.User;

public class UpdateUserDto
{
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O primeiro nome deve ter entre 2 e 50 caracteres")]
    public string? FirstName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "O sobrenome deve ter entre 2 e 50 caracteres")]
    public string? LastName { get; set; }

    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(255, ErrorMessage = "O email não pode exceder 255 caracteres")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Formato de telefone inválido")]
    public string? PhoneNumber { get; set; }

    public UserRole? Role { get; set; }
        
    public UserStatus? Status { get; set; }
        
    public bool? IsEmailVerified { get; set; }
}