using BarraTour.Api.Features.Users.DTOs;
using FluentValidation;

namespace BarraTour.Api.Features.Admins.DTOs;

public class CreateAdminRequestDtoValidator : AbstractValidator<CreateAdminRequestDto>
{
    public CreateAdminRequestDtoValidator()
    {
        RuleFor(x => x.Department)
            .MaximumLength(100).WithMessage("O departamento não pode ter mais de 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Department));

        RuleFor(x => x.User)
            .NotNull().WithMessage("Os dados do usuário são obrigatórios")
            .SetValidator(new CreateUserRequestDtoValidator()!);
    }
}