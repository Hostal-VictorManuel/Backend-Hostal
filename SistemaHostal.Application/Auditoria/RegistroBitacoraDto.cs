namespace SistemaHostal.Application.Auditoria;

public record RegistroBitacoraDto(
    int Id,
    int UsuarioId,
    string NombreUsuario,
    string Modulo,
    string TipoOperacion,
    string Detalle,
    DateTime FechaHora);