namespace SistemaHostal.Application.Inventario;

public record MovimientoInventarioDto(
    int Id,
    int ProductoId,
    string NombreProducto,
    string Tipo,
    int Cantidad,
    string? Motivo,
    int? VentaId,
    int UsuarioId,
    string NombreUsuario,
    DateTime FechaHora);