using FluentValidation;

namespace SistemaHostal.Application.Catalogo;

public class RegistrarProductoCommandValidator : AbstractValidator<RegistrarProductoCommand>
{
    public RegistrarProductoCommandValidator()
    {
        RuleFor(x => x.CodigoBarras).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CategoriaId).GreaterThan(0);
        RuleFor(x => x.Precio).GreaterThan(0);
        RuleFor(x => x.StockInicial).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockMinimo).GreaterThanOrEqualTo(0);
    }
}