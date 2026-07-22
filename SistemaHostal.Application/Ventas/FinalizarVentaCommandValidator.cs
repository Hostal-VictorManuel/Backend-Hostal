using FluentValidation;

namespace SistemaHostal.Application.Ventas;

public class FinalizarVentaCommandValidator : AbstractValidator<FinalizarVentaCommand>
{
    public FinalizarVentaCommandValidator()
    {
        RuleForEach(x => x.Pagos).ChildRules(pago =>
        {
            pago.RuleFor(p => p.Monto).GreaterThan(0);
        });
    }
}