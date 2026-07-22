using FluentValidation;

namespace SistemaHostal.Application.Inventario;

public class RegistrarAjusteCommandValidator : AbstractValidator<RegistrarAjusteCommand>
{
    public RegistrarAjusteCommandValidator()
    {
        RuleFor(x => x.NuevoStock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(250);
    }
}