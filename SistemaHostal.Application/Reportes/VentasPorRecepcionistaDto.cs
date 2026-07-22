namespace SistemaHostal.Application.Reportes;

public record VentasPorRecepcionistaDto(int UsuarioId, string NombreUsuario, int CantidadVentas, decimal Total);