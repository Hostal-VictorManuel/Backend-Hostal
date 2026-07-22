namespace SistemaHostal.Application.Reportes;

public record VentasPorDiaDto(DateTime Fecha, int CantidadVentas, decimal Total);