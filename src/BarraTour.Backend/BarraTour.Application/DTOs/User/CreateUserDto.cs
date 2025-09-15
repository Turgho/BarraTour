using System.ComponentModel.DataAnnotations;
using BarraTour.Domain.Enums;

namespace BarraTour.Application.DTOs.User;

public class CreateUserDto
{
    [Required(ErrorMessage = "O primeiro nome é obrigatório")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O primeiro nome deve ter entre 2 e 50 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O sobrenome é obrigatório")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O sobrenome deve ter entre 2 e 50 caracteres")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(255, ErrorMessage = "O email não pode exceder 255 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número de telefone é obrigatório")]
    [Phone(ErrorMessage = "Formato de telefone inválido")]
    public string PhoneNumber { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Tourist;
}