namespace SistemaHostal.Application.Ventas;

public record VentaResumenDto(
    int Id,
    string NumeroVenta,
    int TurnoId,
    string? NumeroHabitacion,
    decimal Total,
    string Estado,
    DateTime FechaHoraInicio,
    DateTime? FechaHoraFinalizacion);