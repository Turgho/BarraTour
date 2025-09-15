using BarraTour.Domain.ValueObjects.User;
using FluentValidation;

namespace BarraTour.Application.Validators.User;

public class UserNameValidator : AbstractValidator<UserName>
{
    public UserNameValidator()
    {
        RuleFor(name => name.FirstName)
            .NotEmpty().WithMessage("Primeiro nome é obrigatório")
            .Length(2, 50).WithMessage("Primeiro nome deve ter entre 2 e 50 caracteres")
            .Matches(@"^[\p{L}\s\-'.]+$").WithMessage("Primeiro nome contém caracteres inválidos");

        RuleFor(name => name.LastName)
            .NotEmpty().WithMessage("Sobrenome é obrigatório")
            .Length(2, 50).WithMessage("Sobrenome deve ter entre 2 e 50 caracteres")
            .Matches(@"^[\p{L}\s\-'.]+$").WithMessage("Sobrenome contém caracteres inválidos");
    }
}