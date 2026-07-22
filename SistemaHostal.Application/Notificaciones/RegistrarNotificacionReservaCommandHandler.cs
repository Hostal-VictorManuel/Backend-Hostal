using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Application.Notificaciones;

public class RegistrarNotificacionReservaCommandHandler(
    INotificacionRepository notificacionRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarNotificacionReservaCommand, Result<NotificacionDto>>
{
    public async Task<Result<NotificacionDto>> Handle(RegistrarNotificacionReservaCommand request, CancellationToken cancellationToken)
    {
        var notificacion = new Notificacion(request.Canal, request.Contenido);
        await notificacionRepository.AddAsync(notificacion, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new NotificacionDto(notificacion.Id, notificacion.Canal, notificacion.Contenido, notificacion.Estado.ToString(), notificacion.FechaRecepcion);
        return Result<NotificacionDto>.Success(dto);
    }
}