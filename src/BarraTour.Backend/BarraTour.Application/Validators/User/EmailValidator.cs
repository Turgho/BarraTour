using System.Text.RegularExpressions;
using BarraTour.Domain.ValueObjects.User;
using FluentValidation;
using FluentValidation.Validators;

namespace BarraTour.Application.Validators.User;

public class EmailValidator : AbstractValidator<Email>
{
    public EmailValidator()
    {
        RuleFor(email => email.Value)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Formato de email inválido")
            .MaximumLength(254).WithMessage("Email não pode exceder 254 caracteres");
    }
}