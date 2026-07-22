namespace SistemaHostal.Application.Reportes;

public record IngresoPorTurnoDto(int TurnoId, string NombreUsuario, DateTime FechaHoraInicio, DateTime? FechaHoraFin, decimal Total);