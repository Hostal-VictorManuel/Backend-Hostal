using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.API.Resources.Inventario;
using SistemaHostal.Application.Inventario;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/movimientos-inventario")]
[Authorize]
public class MovimientosInventarioController(IMediator mediator, IMovimientoInventarioQueries movimientoQueries) : ControllerBase
{
    [HttpPost("ingresos")]
    public async Task<IActionResult> RegistrarIngreso(RegistrarIngresoResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var command = new RegistrarIngresoCommand(resource.ProductoId, resource.Cantidad, usuarioId);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }

    [HttpPost("ajustes")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> RegistrarAjuste(RegistrarAjusteResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var command = new RegistrarAjusteCommand(resource.ProductoId, resource.NuevoStock, resource.Motivo, usuarioId);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Listar([FromQuery] int? productoId, [FromQuery] DateTime? fecha, CancellationToken cancellationToken)
    {
        var movimientos = await movimientoQueries.ListarAsync(productoId, fecha, cancellationToken);
        return Ok(movimientos);
    }
}