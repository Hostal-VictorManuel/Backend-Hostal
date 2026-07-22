using FluentValidation;

namespace SistemaHostal.Application.Ventas;

public class AgregarProductoCommandValidator : AbstractValidator<AgregarProductoCommand>
{
    public AgregarProductoCommandValidator()
    {
        RuleFor(x => x.Cantidad).GreaterThan(0);
    }
}