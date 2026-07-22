using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Notificaciones;
using SistemaHostal.Domain.Notificaciones;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Notificaciones;

public class NotificacionQueries(SistemaHostalDbContext context) : INotificacionQueries
{
    public async Task<IReadOnlyList<NotificacionDto>> ListarAsync(EstadoNotificacion? estado, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Notificacion>().AsNoTracking().AsQueryable();

        if (estado.HasValue)
            query = query.Where(n => n.Estado == estado.Value);

        return await query
            .OrderByDescending(n => n.FechaRecepcion)
            .Select(n => new NotificacionDto(n.Id, n.Canal, n.Contenido, n.Estado.ToString(), n.FechaRecepcion))
            .ToListAsync(cancellationToken);
    }
}