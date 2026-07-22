using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Application.Turnos;

public interface ITurnoRepository : IRepository<Turno>
{
    Task<Turno?> ObtenerTurnoActivoAsync(int usuarioId, CancellationToken cancellationToken = default);

    Task<Turno?> ObtenerConIncidenciasAsync(int turnoId, CancellationToken cancellationToken = default);
}