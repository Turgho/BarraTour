using System.Text.RegularExpressions;
using FluentValidation;

namespace BarraTour.Application.Validators.User;

public class UserValidator : AbstractValidator<Domain.Entities.User>
{
    public UserValidator()
    {
        // Validação do UserId
        RuleFor(user => user.UserId)
            .NotEmpty().WithMessage("ID do usuário é obrigatório")
            .NotEqual(Guid.Empty).WithMessage("ID do usuário não pode ser vazio");

        // Validação do Name (objeto de valor)
        RuleFor(user => user.Name)
            .NotNull().WithMessage("Nome é obrigatório")
            .SetValidator(new UserNameValidator());

        // Validação do Email (objeto de valor)
        RuleFor(user => user.Email)
            .NotNull().WithMessage("Email é obrigatório")
            .SetValidator(new EmailValidator());

        // Validação do PasswordHash (objeto de valor)
        RuleFor(user => user.PasswordHash)
            .NotNull().WithMessage("Hash de senha é obrigatório")
            .SetValidator(new PasswordHashValidator());

        // Validação do PhoneNumber (objeto de valor)
        RuleFor(user => user.PhoneNumber)
            .NotNull().WithMessage("Número de telefone é obrigatório")
            .SetValidator(new PhoneNumberValidator());

        // Validação da Role
        RuleFor(user => user.Role)
            .IsInEnum().WithMessage("Tipo de usuário deve ser um valor válido");

        // Validação do Status
        RuleFor(user => user.Status)
            .IsInEnum().WithMessage("Status deve ser um valor válido");

        // Validação do LastLogin
        RuleFor(user => user.LastLogin)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Último login não pode ser no futuro")
            .When(user => user.LastLogin.HasValue);

        // Validação do CreatedAt
        RuleFor(user => user.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data de criação não pode ser no futuro");
    }
}