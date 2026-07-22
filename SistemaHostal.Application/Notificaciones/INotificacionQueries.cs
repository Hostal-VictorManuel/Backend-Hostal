using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Application.Notificaciones;

public interface INotificacionQueries
{
    Task<IReadOnlyList<NotificacionDto>> ListarAsync(EstadoNotificacion? estado, CancellationToken cancellationToken = default);
}