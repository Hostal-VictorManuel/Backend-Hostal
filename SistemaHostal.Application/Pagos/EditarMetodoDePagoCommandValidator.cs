using FluentValidation;

namespace SistemaHostal.Application.Pagos;

public class EditarMetodoDePagoCommandValidator : AbstractValidator<EditarMetodoDePagoCommand>
{
    public EditarMetodoDePagoCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
    }
}