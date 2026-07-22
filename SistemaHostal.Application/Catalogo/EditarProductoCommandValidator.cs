using FluentValidation;

namespace SistemaHostal.Application.Catalogo;

public class EditarProductoCommandValidator : AbstractValidator<EditarProductoCommand>
{
    public EditarProductoCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
        RuleFor(x => x.Precio).GreaterThan(0);
        RuleFor(x => x.StockMinimo).GreaterThanOrEqualTo(0);
    }
}