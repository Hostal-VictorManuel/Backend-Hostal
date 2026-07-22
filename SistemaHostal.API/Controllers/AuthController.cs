using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SistemaHostal.API.Resources.Identidad;
using SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator, IPublisher publisher) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginResource resource, CancellationToken cancellationToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
        var command = new LoginCommand(resource.NombreUsuario, resource.Password, ip);
        var result = await mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new { message = result.Message });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var nombreCompleto = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;

        await publisher.Publish(new UsuarioDesconectado(usuarioId, nombreCompleto), cancellationToken);

        return Ok(new { message = "Sesión cerrada." });
    }
}