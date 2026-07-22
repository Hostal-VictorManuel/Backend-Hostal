using FluentValidation;

namespace SistemaHostal.Application.Identidad;

public class RestablecerPasswordCommandValidator : AbstractValidator<RestablecerPasswordCommand>
{
    public RestablecerPasswordCommandValidator()
    {
        RuleFor(x => x.NuevaPassword).NotEmpty().MinimumLength(6);
    }
}