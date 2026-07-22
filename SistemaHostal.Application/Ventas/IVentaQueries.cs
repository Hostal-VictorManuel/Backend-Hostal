namespace SistemaHostal.Application.Ventas;

public interface IVentaQueries
{
    Task<IReadOnlyList<VentaResumenDto>> BuscarAsync(
        DateTime? fecha, string? numeroVenta, int? turnoId, CancellationToken cancellationToken = default);

    Task<VentaDetalleDto?> ObtenerDetalleAsync(int ventaId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HabitacionConsumoPendienteDto>> ObtenerHabitacionesConConsumosPendientesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VentaDetalleDto>> ObtenerConsumosPorHabitacionAsync(string numeroHabitacion, CancellationToken cancellationToken = default);
}