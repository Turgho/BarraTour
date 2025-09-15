using BarraTour.Domain.ValueObjects.User;
using FluentValidation;

namespace BarraTour.Application.Validators.User;

public class PasswordHashValidator : AbstractValidator<PasswordHash>
{
    public PasswordHashValidator()
    {
        RuleFor(password => password.Value)
            .NotEmpty().WithMessage("Hash de senha é obrigatório")
            .MinimumLength(20).WithMessage("Hash de senha parece inválido");
    }
}