using FluentValidation;

namespace SistemaHostal.Application.Turnos;

public class RegistrarIncidenciaCommandValidator : AbstractValidator<RegistrarIncidenciaCommand>
{
    public RegistrarIncidenciaCommandValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(500);
    }
}