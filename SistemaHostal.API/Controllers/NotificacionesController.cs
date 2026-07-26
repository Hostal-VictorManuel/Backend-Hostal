using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.Application.Notificaciones;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/notificaciones")]
[Authorize]
public class NotificacionesController(IMediator mediator, INotificacionQueries notificacionQueries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] EstadoNotificacion? estado, CancellationToken cancellationToken)
    {
        var rolUsuario = Enum.Parse<RolUsuario>(User.FindFirstValue(ClaimTypes.Role)!);
        var notificaciones = await notificacionQueries.ListarAsync(estado, rolUsuario, cancellationToken);
        return Ok(notificaciones);
    }

    [HttpPatch("{id:int}/leida")]
    public async Task<IActionResult> MarcarLeida(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new MarcarNotificacionLeidaCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }
}