using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SistemaHostal.API.Resources.Ventas;
using SistemaHostal.Application.Ventas;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/ventas")]
[Authorize]
public class VentasController(IMediator mediator, IVentaQueries ventaQueries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Iniciar(IniciarVentaResource resource, CancellationToken cancellationToken)
    {
        var command = new IniciarVentaCommand(resource.TurnoId, resource.NumeroHabitacion);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(ObtenerDetalle), new { id = result.Value!.Id }, result.Value)
            : Conflict(new { message = result.Message });
    }

    [HttpPost("{id:int}/productos")]
    public async Task<IActionResult> AgregarProducto(int id, AgregarProductoResource resource, CancellationToken cancellationToken)
    {
        var command = new AgregarProductoCommand(id, resource.ProductoId, resource.Cantidad);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }

    [HttpPut("{id:int}/lineas/{lineaId:int}")]
    public async Task<IActionResult> ModificarCantidad(int id, int lineaId, ModificarCantidadResource resource, CancellationToken cancellationToken)
    {
        var command = new ModificarCantidadCommand(id, lineaId, resource.NuevaCantidad);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }

    [HttpDelete("{id:int}/lineas/{lineaId:int}")]
    public async Task<IActionResult> EliminarLinea(int id, int lineaId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new EliminarLineaCommand(id, lineaId), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/observaciones")]
    public async Task<IActionResult> RegistrarObservaciones(int id, RegistrarObservacionesResource resource, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RegistrarObservacionesCommand(id, resource.Observaciones), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CancelarVentaCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : Conflict(new { message = result.Message });
    }

    [HttpPatch("{id:int}/finalizar")]
    public async Task<IActionResult> Finalizar(int id, FinalizarVentaResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var pagos = resource.Pagos.Select(p => new PagoInput(p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList();
        var command = new FinalizarVentaCommand(id, pagos, resource.CargarAHabitacion, usuarioId);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }
    
    [HttpPatch("{id:int}/marcar-pagada")]
    public async Task<IActionResult> MarcarComoPagada(int id, MarcarComoPagadaResource resource, CancellationToken cancellationToken)
    {
        var pagos = resource.Pagos.Select(p => new PagoInput(p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList();
        var result = await mediator.Send(new MarcarVentaComoPagadaCommand(id, pagos), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }

    [HttpGet("habitaciones-pendientes")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ObtenerHabitacionesConConsumosPendientes(CancellationToken cancellationToken)
    {
        var habitaciones = await ventaQueries.ObtenerHabitacionesConConsumosPendientesAsync(cancellationToken);
        return Ok(habitaciones);
    }

    [HttpGet("habitaciones/{numeroHabitacion}/consumos")]
    public async Task<IActionResult> ObtenerConsumosPorHabitacion(string numeroHabitacion, CancellationToken cancellationToken)
    {
        var consumos = await ventaQueries.ObtenerConsumosPorHabitacionAsync(numeroHabitacion, cancellationToken);
        return Ok(consumos);
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Buscar(
        [FromQuery] DateTime? fecha, [FromQuery] string? numeroVenta, [FromQuery] int? turnoId, CancellationToken cancellationToken)
    {
        var ventas = await ventaQueries.BuscarAsync(fecha, numeroVenta, turnoId, cancellationToken);
        return Ok(ventas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerDetalle(int id, CancellationToken cancellationToken)
    {
        var venta = await ventaQueries.ObtenerDetalleAsync(id, cancellationToken);
        return venta is null ? NotFound() : Ok(venta);
    }
}