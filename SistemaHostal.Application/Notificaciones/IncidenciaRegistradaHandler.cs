using System.Text.Json;
using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Turnos;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Application.Notificaciones;

public class IncidenciaRegistradaHandler(
    INotificacionRepository notificacionRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<IncidenciaRegistrada>
{
    public async Task Handle(IncidenciaRegistrada notification, CancellationToken cancellationToken)
    {
        var contenido = JsonSerializer.Serialize(new
        {
            turnoId = notification.TurnoId,
            nombreUsuario = notification.NombreUsuario,
            descripcion = notification.Descripcion
        });

        var notificacion = new Notificacion("IncidenciaTurno", contenido, rolDestino: null);
        await notificacionRepository.AddAsync(notificacion, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}