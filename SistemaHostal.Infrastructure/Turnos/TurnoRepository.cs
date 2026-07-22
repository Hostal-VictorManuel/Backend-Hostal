using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Turnos;
using SistemaHostal.Domain.Turnos;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Turnos;

public class TurnoRepository(SistemaHostalDbContext context) : Repository<Turno>(context), ITurnoRepository
{
    public async Task<Turno?> ObtenerTurnoActivoAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Turno>()
            .FirstOrDefaultAsync(t => t.UsuarioId == usuarioId && t.Estado == EstadoTurno.Activo, cancellationToken);
    }

    public async Task<Turno?> ObtenerConIncidenciasAsync(int turnoId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Turno>()
            .Include(t => t.Incidencias)
            .FirstOrDefaultAsync(t => t.Id == turnoId, cancellationToken);
    }
}