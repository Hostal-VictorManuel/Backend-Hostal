using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.API.Resources.Identidad;
using SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Roles = "Administrador")]
public class UsuariosController(IMediator mediator, IUsuarioQueries usuarioQueries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(ObtenerDetalle), new { id = result.Value!.Id }, result.Value)
            : Conflict(new { message = result.Message });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Editar(int id, EditarUsuarioResource resource, CancellationToken cancellationToken)
    {
        var command = new EditarUsuarioCommand(id, resource.NombreCompleto, resource.NombreUsuario, resource.Rol);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/activar")]
    public async Task<IActionResult> Activar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarUsuarioCommand(id, true), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/desactivar")]
    public async Task<IActionResult> Desactivar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarUsuarioCommand(id, false), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/restablecer-password")]
    public async Task<IActionResult> RestablecerPassword(int id, RestablecerPasswordResource resource, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RestablecerPasswordCommand(id, resource.NuevaPassword), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Buscar(
        [FromQuery] string? texto,
        [FromQuery] RolUsuario? rol,
        [FromQuery] EstadoUsuario? estado,
        CancellationToken cancellationToken)
    {
        var usuarios = await usuarioQueries.BuscarAsync(texto, rol, estado, cancellationToken);
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerDetalle(int id, CancellationToken cancellationToken)
    {
        var usuario = await usuarioQueries.ObtenerDetalleAsync(id, cancellationToken);
        return usuario is null ? NotFound() : Ok(usuario);
    }
}