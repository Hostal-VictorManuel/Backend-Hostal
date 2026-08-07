using FluentValidation;

namespace SistemaHostal.Application.Ventas;

public class TransferirHabitacionCommandValidator : AbstractValidator<TransferirHabitacionCommand>
{
    public TransferirHabitacionCommandValidator()
    {
        RuleFor(x => x.NumeroHabitacionNueva).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(500);
    }
}