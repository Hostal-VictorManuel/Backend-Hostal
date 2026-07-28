using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.API.Resources.Trabajadores;
using SistemaHostal.Application.Trabajadores;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/trabajadores")]
[Authorize]
public class TrabajadoresController(IMediator mediator, ITrabajadorQueries trabajadorQueries) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Registrar(RegistrarTrabajadorResource resource, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegistrarTrabajadorCommand(resource.Nombre), cancellationToken);
        return result.IsSuccess ? Created(string.Empty, result.Value) : Conflict(new { message = result.Message });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id, EditarTrabajadorResource resource, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new EditarTrabajadorCommand(id, resource.Nombre), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/activar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Activar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarTrabajadorCommand(id, true), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/desactivar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Desactivar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarTrabajadorCommand(id, false), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] string? texto, CancellationToken cancellationToken)
    {
        var trabajadores = await trabajadorQueries.BuscarAsync(texto, cancellationToken);
        return Ok(trabajadores);
    }
}