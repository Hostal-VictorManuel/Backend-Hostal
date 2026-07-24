namespace SistemaHostal.Application.Turnos;

public interface ITurnoQueries
{
    Task<IReadOnlyList<TurnoResumenDto>> ListarAsync(CancellationToken cancellationToken = default);

    Task<TurnoDetalleDto?> ObtenerDetalleAsync(int turnoId, CancellationToken cancellationToken = default);

    Task<TurnoDetalleDto?> ObtenerActivoAsync(int usuarioId, CancellationToken cancellationToken = default);
    
}