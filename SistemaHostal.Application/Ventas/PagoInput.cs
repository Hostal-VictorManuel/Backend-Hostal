namespace SistemaHostal.Application.Ventas;

public record PagoInput(int MetodoDePagoId, decimal Monto, string? ReferenciaPago);