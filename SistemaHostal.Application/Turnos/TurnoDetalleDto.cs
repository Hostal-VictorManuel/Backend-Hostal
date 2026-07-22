namespace SistemaHostal.Application.Turnos;

public record TurnoDetalleDto(
    int Id,
    int UsuarioId,
    string NombreUsuario,
    DateTime FechaHoraInicio,
    DateTime? FechaHoraFin,
    string Estado,
    IReadOnlyList<IncidenciaDto> Incidencias);