using FluentValidation;

namespace SistemaHostal.Application.Inventario;

public class RegistrarIngresoCommandValidator : AbstractValidator<RegistrarIngresoCommand>
{
    public RegistrarIngresoCommandValidator()
    {
        RuleFor(x => x.Cantidad).GreaterThan(0);
    }
}