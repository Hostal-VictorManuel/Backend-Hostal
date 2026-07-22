namespace SistemaHostal.Application.Turnos;

public record TurnoResumenDto(
    int Id,
    int UsuarioId,
    string NombreUsuario,
    DateTime FechaHoraInicio,
    DateTime? FechaHoraFin,
    string Estado);