using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.API.Seguridad;
using SistemaHostal.Application.Notificaciones;

namespace SistemaHostal.API.Webhooks;

[ApiController]
[Route("api/webhooks/n8n")]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationHandler.SchemeName)]
public class N8nWebhooksController(IMediator mediator) : ControllerBase
{
    [HttpPost("reservas")]
    public async Task<IActionResult> RecibirReserva(RegistrarNotificacionReservaCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { message = result.Message });
    }
}