using FluentValidation;

namespace SistemaHostal.Application.Ventas;

public class AnularVentaCommandValidator : AbstractValidator<AnularVentaCommand>
{
    public AnularVentaCommandValidator()
    {
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(500);
    }
}