namespace SistemaHostal.Application.Ventas;

public record PagoVentaDto(int Id, int MetodoDePagoId, decimal Monto, string? ReferenciaPago);