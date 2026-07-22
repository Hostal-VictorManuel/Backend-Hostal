namespace SistemaHostal.Application.Reportes;

public record ProductoMasVendidoDto(int ProductoId, string NombreProducto, int CantidadVendida, decimal TotalVendido);