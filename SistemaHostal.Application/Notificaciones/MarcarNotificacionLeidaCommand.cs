using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Notificaciones;

public record MarcarNotificacionLeidaCommand(int NotificacionId) : IRequest<Result>;