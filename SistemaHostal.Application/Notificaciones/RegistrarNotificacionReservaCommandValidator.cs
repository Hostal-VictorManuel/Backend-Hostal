using FluentValidation;

namespace SistemaHostal.Application.Notificaciones;

public class RegistrarNotificacionReservaCommandValidator : AbstractValidator<RegistrarNotificacionReservaCommand>
{
    public RegistrarNotificacionReservaCommandValidator()
    {
        RuleFor(x => x.Canal).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Contenido).NotEmpty();
    }
}