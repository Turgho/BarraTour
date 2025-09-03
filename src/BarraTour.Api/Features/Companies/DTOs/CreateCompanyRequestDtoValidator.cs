using BarraTour.Api.Features.Users.DTOs;
using FluentValidation;

namespace BarraTour.Api.Features.Companies.DTOs;

public class CreateCompanyRequestDtoValidator : AbstractValidator<CreateCompanyRequestDto>
{
    public CreateCompanyRequestDtoValidator()
    {
        RuleFor(x => x.CNPJ)
            .NotEmpty().WithMessage("O CNPJ é obrigatório")
            .Length(14).WithMessage("CNPJ deve ter 14 dígitos")
            .IsValidCNPJ().WithMessage("CNPJ inválido");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da empresa é obrigatório")
            .MaximumLength(150).WithMessage("O nome da empresa não pode ter mais de 150 caracteres");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("O endereço é obrigatório")
            .MaximumLength(255).WithMessage("O endereço não pode ter mais de 255 caracteres");

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("O telefone não pode ter mais de 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("E-mail inválido")
            .MaximumLength(150).WithMessage("O e-mail não pode ter mais de 150 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("A descrição não pode ter mais de 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.LogoUrl)
            .MaximumLength(255).WithMessage("A URL do logo não pode ter mais de 255 caracteres")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl));

        RuleFor(x => x.User)
            .NotNull().WithMessage("Os dados do usuário são obrigatórios")
            .SetValidator(new CreateUserRequestDtoValidator());
    }
}