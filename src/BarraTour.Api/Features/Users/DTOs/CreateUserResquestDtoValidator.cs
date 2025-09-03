using FluentValidation;

namespace BarraTour.Api.Features.Users.DTOs;

public class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("E-mail inválido")
            .MaximumLength(150).WithMessage("O e-mail não pode ter mais de 150 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(6).WithMessage("A senha precisa ter pelo menos 6 caracteres");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("O telefone não pode ter mais de 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Phone));
    }
}