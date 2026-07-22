namespace SistemaHostal.API.Resources.Ventas;

public record PagoResource(int MetodoDePagoId, decimal Monto, string? ReferenciaPago);

public record FinalizarVentaResource(List<PagoResource> Pagos, bool CargarAHabitacion);