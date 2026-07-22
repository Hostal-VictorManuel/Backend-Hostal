namespace SistemaHostal.Application.Notificaciones;

public record NotificacionDto(int Id, string Canal, string Contenido, string Estado, DateTime FechaRecepcion);