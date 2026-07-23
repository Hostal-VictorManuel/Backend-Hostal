using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Turnos;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Turnos;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Turnos;

public class TurnoQueries(SistemaHostalDbContext context) : ITurnoQueries
{
    public async Task<IReadOnlyList<TurnoResumenDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var query =
            from t in context.Set<Turno>().AsNoTracking()
            join u in context.Set<Usuario>().AsNoTracking() on t.UsuarioId equals u.Id
            orderby t.FechaHoraInicio descending
            select new TurnoResumenDto(t.Id, t.UsuarioId, u.NombreCompleto, t.FechaHoraInicio, t.FechaHoraFin, t.Estado.ToString());

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TurnoDetalleDto?> ObtenerDetalleAsync(int turnoId, CancellationToken cancellationToken = default)
    {
        var turno = await context.Set<Turno>()
            .AsNoTracking()
            .Include(t => t.Incidencias)
            .FirstOrDefaultAsync(t => t.Id == turnoId, cancellationToken);

        if (turno is null) return null;

        var usuario = await context.Set<Usuario>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == turno.UsuarioId, cancellationToken);

        var incidencias = turno.Incidencias
            .Select(i => new IncidenciaDto(i.Id, i.Descripcion, i.FechaHora))
            .ToList();

        return new TurnoDetalleDto(
            turno.Id, turno.UsuarioId, usuario?.NombreCompleto ?? string.Empty,
            turno.FechaHoraInicio, turno.FechaHoraFin, turno.Estado.ToString(), incidencias);
    }

    public async Task<TurnoDetalleDto?> ObtenerActivoAsync(CancellationToken cancellationToken = default)
    {
        var turno = await context.Set<Turno>()
            .AsNoTracking()
            .Include(t => t.Incidencias)
            .FirstOrDefaultAsync(t => t.Estado == EstadoTurno.Activo, cancellationToken);

        if (turno is null) return null;

        var usuario = await context.Set<Usuario>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == turno.UsuarioId, cancellationToken);

        var incidencias = turno.Incidencias
            .Select(i => new IncidenciaDto(i.Id, i.Descripcion, i.FechaHora))
            .ToList();

        return new TurnoDetalleDto(
            turno.Id, turno.UsuarioId, usuario?.NombreCompleto ?? string.Empty,
            turno.FechaHoraInicio, turno.FechaHoraFin, turno.Estado.ToString(), incidencias);
    }
}