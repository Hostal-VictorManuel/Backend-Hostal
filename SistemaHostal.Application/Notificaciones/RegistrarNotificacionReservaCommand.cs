using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Notificaciones;

public record RegistrarNotificacionReservaCommand(string Canal, string Contenido) : IRequest<Result<NotificacionDto>>;