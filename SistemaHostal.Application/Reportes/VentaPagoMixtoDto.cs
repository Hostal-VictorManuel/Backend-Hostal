namespace SistemaHostal.Application.Reportes;

public record VentaPagoMixtoDto(int VentaId, string NumeroVenta, int CantidadMetodosPago, decimal Total);