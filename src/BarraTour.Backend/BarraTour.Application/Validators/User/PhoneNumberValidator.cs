using BarraTour.Domain.ValueObjects.User;
using FluentValidation;

namespace BarraTour.Application.Validators.User;

public class PhoneNumberValidator : AbstractValidator<PhoneNumber>
{
    public PhoneNumberValidator()
    {
        RuleFor(phone => phone.CountryCode)
            .NotEmpty().WithMessage("Código do país é obrigatório")
            .Matches(@"^\+[0-9]{1,3}$").WithMessage("Código do país deve estar no formato +XXX");

        RuleFor(phone => phone.Number)
            .NotEmpty().WithMessage("Número de telefone é obrigatório")
            .MinimumLength(8).WithMessage("Número deve ter pelo menos 8 dígitos")
            .Matches(@"^[0-9]+$").WithMessage("Número deve conter apenas dígitos");
    }
}