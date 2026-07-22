namespace SistemaHostal.Application.Reportes;

public record VentasPorMetodoPagoDto(int MetodoDePagoId, string NombreMetodoDePago, decimal Total);