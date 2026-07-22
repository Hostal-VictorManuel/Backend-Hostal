namespace SistemaHostal.Application.Ventas;

public record LineaVentaDto(int Id, int ProductoId, string NombreProducto, decimal PrecioUnitario, int Cantidad, decimal Subtotal);