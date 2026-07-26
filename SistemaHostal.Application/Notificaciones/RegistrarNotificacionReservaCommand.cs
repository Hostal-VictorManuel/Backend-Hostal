using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Notificaciones;

public record RegistrarNotificacionReservaCommand(string Canal, string Contenido, RolUsuario? RolDestino) : IRequest<Result<NotificacionDto>>;