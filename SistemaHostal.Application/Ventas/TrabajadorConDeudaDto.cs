namespace SistemaHostal.Application.Ventas;

public record TrabajadorConDeudaDto(int TrabajadorId, string NombreTrabajador, int CantidadVentasPendientes, decimal TotalPendiente);