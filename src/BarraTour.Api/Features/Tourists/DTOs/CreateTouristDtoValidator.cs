using BarraTour.Api.Features.Users.DTOs;
using FluentValidation;

namespace BarraTour.Api.Features.Tourists.DTOs;

public class CreateTouristRequestDtoValidator : AbstractValidator<CreateTouristRequestDto>
{
    public CreateTouristRequestDtoValidator()
    {
        RuleFor(x => x.CPF)
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .Length(11).WithMessage("CPF deve ter 11 dígitos")
            .IsValidCPF().WithMessage("CPF inválido");

        RuleFor(x => x.EmergencyContactName)
            .MaximumLength(100).WithMessage("O nome do contato de emergência não pode ter mais de 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.EmergencyContactName));

        RuleFor(x => x.EmergencyContactPhone)
            .MaximumLength(20).WithMessage("O telefone de emergência não pode ter mais de 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.EmergencyContactPhone));

        RuleFor(x => x.Preferences)
            .MaximumLength(500).WithMessage("As preferências não podem ter mais de 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Preferences));

        RuleFor(x => x.User)
            .NotNull().WithMessage("Os dados do usuário são obrigatórios")
            .SetValidator(new CreateUserRequestDtoValidator());
    }
}