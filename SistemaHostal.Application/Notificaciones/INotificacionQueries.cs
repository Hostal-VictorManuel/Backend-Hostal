using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Application.Notificaciones;

public interface INotificacionQueries
{
    Task<IReadOnlyList<NotificacionDto>> ListarAsync(EstadoNotificacion? estado, RolUsuario rolUsuarioActual, CancellationToken cancellationToken = default);
}