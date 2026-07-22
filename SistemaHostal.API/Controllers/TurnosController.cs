using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.Application.Turnos;
using SistemaHostal.API.Resources.Turnos;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/turnos")]
[Authorize]
public class TurnosController(IMediator mediator, ITurnoQueries turnoQueries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Iniciar(CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

        var result = await mediator.Send(new IniciarTurnoCommand(usuarioId), cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(ObtenerDetalle), new { id = result.Value!.Id }, result.Value)
            : Conflict(new { message = result.Message });
    }

    [HttpPatch("{id:int}/finalizar")]
    public async Task<IActionResult> Finalizar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FinalizarTurnoCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPost("{id:int}/incidencias")]
    public async Task<IActionResult> RegistrarIncidencia(int id, RegistrarIncidenciaResource resource, CancellationToken cancellationToken)
    {
        var command = new RegistrarIncidenciaCommand(id, resource.Descripcion);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var turnos = await turnoQueries.ListarAsync(cancellationToken);
        return Ok(turnos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerDetalle(int id, CancellationToken cancellationToken)
    {
        var turno = await turnoQueries.ObtenerDetalleAsync(id, cancellationToken);
        return turno is null ? NotFound() : Ok(turno);
    }

    [HttpGet("activo")]
    public async Task<IActionResult> ObtenerActivo(CancellationToken cancellationToken)
    {
        var turno = await turnoQueries.ObtenerActivoAsync(cancellationToken);
        return turno is null ? NotFound() : Ok(turno);
    }
}