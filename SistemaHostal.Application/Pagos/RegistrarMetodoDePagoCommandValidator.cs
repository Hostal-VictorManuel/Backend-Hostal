using FluentValidation;

namespace SistemaHostal.Application.Pagos;

public class RegistrarMetodoDePagoCommandValidator : AbstractValidator<RegistrarMetodoDePagoCommand>
{
    public RegistrarMetodoDePagoCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
    }
}