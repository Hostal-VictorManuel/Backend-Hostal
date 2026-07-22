using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Application.Notificaciones;

public class MarcarNotificacionLeidaCommandHandler(
    INotificacionRepository notificacionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<MarcarNotificacionLeidaCommand, Result>
{
    public async Task<Result> Handle(MarcarNotificacionLeidaCommand request, CancellationToken cancellationToken)
    {
        var notificacion = await notificacionRepository.GetByIdAsync(request.NotificacionId, cancellationToken);
        if (notificacion is null)
            return Result.Failure(NotificacionesError.NotificacionNoEncontrada, "Notificación no encontrada.");

        notificacion.MarcarComoLeida();
        notificacionRepository.Update(notificacion);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}