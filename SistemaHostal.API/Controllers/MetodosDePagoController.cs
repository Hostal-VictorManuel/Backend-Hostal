using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.Application.Pagos;
using SistemaHostal.API.Resources.Pagos;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/metodos-de-pago")]
[Authorize]
public class MetodosDePagoController(IMediator mediator, IMetodoDePagoQueries metodoDePagoQueries) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Registrar(RegistrarMetodoDePagoCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(Buscar), result.Value)
            : Conflict(new { message = result.Message });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id, EditarMetodoDePagoResource resource, CancellationToken cancellationToken)
    {
        var command = new EditarMetodoDePagoCommand(id, resource.Nombre);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/activar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Activar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarMetodoDePagoCommand(id, true), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/desactivar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Desactivar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarMetodoDePagoCommand(id, false), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] string? texto, CancellationToken cancellationToken)
    {
        var metodos = await metodoDePagoQueries.BuscarAsync(texto, cancellationToken);
        return Ok(metodos);
    }
}