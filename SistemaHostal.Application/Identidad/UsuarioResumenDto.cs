namespace SistemaHostal.Application.Identidad;

public record UsuarioResumenDto(
    int Id,
    string NombreCompleto,
    string NombreUsuario,
    string Rol,
    string Estado,
    DateTime? UltimoAcceso);