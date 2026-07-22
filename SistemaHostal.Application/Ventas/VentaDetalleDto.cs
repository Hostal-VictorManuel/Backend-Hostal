namespace SistemaHostal.Application.Ventas;

public record VentaDetalleDto(
    int Id,
    string NumeroVenta,
    int TurnoId,
    string? NumeroHabitacion,
    string? Observaciones,
    decimal Total,
    decimal? VueltoEfectivo,
    string Estado,
    DateTime FechaHoraInicio,
    DateTime? FechaHoraFinalizacion,
    IReadOnlyList<LineaVentaDto> Lineas,
    IReadOnlyList<PagoVentaDto> Pagos);