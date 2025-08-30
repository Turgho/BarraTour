using FluentValidation.Validators;

namespace BarraTour.Api.DTOs.Tourists;

using FluentValidation;

public class CreateTouristDtoValidator : AbstractValidator<CreateTouristDto>
{
    public CreateTouristDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("E-mail inválido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(6).WithMessage("A senha precisa ter pelo menos 6 caracteres");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("O telefone é obrigatório")
            .Matches(@"^\d{10,11}$").WithMessage("Telefone inválido, use apenas números (10 ou 11 dígitos)");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .IsValidCPF().WithMessage("CPF inválido");

        RuleFor(x => x.Preferences)
            .MaximumLength(500).WithMessage("Preferências não podem ultrapassar 500 caracteres");
    }
}
