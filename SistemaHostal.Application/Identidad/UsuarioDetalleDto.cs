namespace SistemaHostal.Application.Identidad;

public record UsuarioDetalleDto(
    int Id,
    string NombreCompleto,
    string NombreUsuario,
    string Rol,
    string Estado,
    DateTime FechaCreacion,
    DateTime? UltimoAcceso);