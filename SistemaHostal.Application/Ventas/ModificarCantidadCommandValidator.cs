using FluentValidation;

namespace SistemaHostal.Application.Ventas;

public class ModificarCantidadCommandValidator : AbstractValidator<ModificarCantidadCommand>
{
    public ModificarCantidadCommandValidator()
    {
        RuleFor(x => x.NuevaCantidad).GreaterThan(0);
    }
}