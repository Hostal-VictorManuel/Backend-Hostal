using FluentValidation;

namespace SistemaHostal.Application.Ventas;

public class MarcarVentaComoPagadaCommandValidator : AbstractValidator<MarcarVentaComoPagadaCommand>
{
    public MarcarVentaComoPagadaCommandValidator()
    {
        RuleFor(x => x.Pagos).NotEmpty();
        RuleForEach(x => x.Pagos).ChildRules(pago =>
        {
            pago.RuleFor(p => p.Monto).GreaterThan(0);
        });
    }
}