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
        var command = new IniciarVentaCommand(resource.TurnoId, resource.NumeroHabitacion, resource.TrabajadorId);
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
        var command = new FinalizarVentaCommand(id, pagos, resource.CargarAHabitacion, resource.CargarATrabajador, usuarioId);
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
    public async Task<IActionResult> Buscar(
        [FromQuery] DateTime? fecha, [FromQuery] string? numeroVenta, [FromQuery] int? turnoId, [FromQuery] string? estado, CancellationToken cancellationToken)
    {
        var ventas = await ventaQueries.BuscarAsync(fecha, numeroVenta, turnoId, estado, cancellationToken);
        return Ok(ventas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerDetalle(int id, CancellationToken cancellationToken)
    {
        var venta = await ventaQueries.ObtenerDetalleAsync(id, cancellationToken);
        return venta is null ? NotFound() : Ok(venta);
    }
    
    [HttpGet("trabajadores-con-deuda")] 
    public async Task<IActionResult> ObtenerTrabajadoresConDeuda(CancellationToken cancellationToken)
    {
        var trabajadores = await ventaQueries.ObtenerTrabajadoresConDeudaAsync(cancellationToken);
        return Ok(trabajadores);
    }

    [HttpPost("trabajadores/cerrar-mes")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CerrarMesTrabajadores(CerrarMesTrabajadoresResource resource, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CerrarMesTrabajadoresCommand(resource.MetodoDePagoId), cancellationToken);
        return result.IsSuccess ? Ok(new { ventasCerradas = result.Value }) : Conflict(new { message = result.Message });
    }
    [HttpGet("trabajadores/{trabajadorId:int}/consumos")]
    public async Task<IActionResult> ObtenerConsumosPorTrabajador(int trabajadorId, CancellationToken cancellationToken)
    {
        var consumos = await ventaQueries.ObtenerConsumosPorTrabajadorAsync(trabajadorId, cancellationToken);
        return Ok(consumos);
    }
    [HttpPatch("trabajadores/{trabajadorId:int}/marcar-pagado")]
    public async Task<IActionResult> MarcarTrabajadorComoPagado(int trabajadorId, MarcarTrabajadorComoPagadoResource resource, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new MarcarTrabajadorComoPagadoCommand(trabajadorId, resource.MetodoDePagoId), cancellationToken);
        return result.IsSuccess ? Ok(new { ventasCerradas = result.Value }) : Conflict(new { message = result.Message });
    }
    [HttpPatch("{id:int}/anular")]
    public async Task<IActionResult> Anular(int id, AnularVentaResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var result = await mediator.Send(new AnularVentaCommand(id, resource.Motivo, usuarioId), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }
    [HttpPatch("{id:int}/transferir-habitacion")]
    public async Task<IActionResult> TransferirHabitacion(int id, TransferirHabitacionResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var command = new TransferirHabitacionCommand(id, resource.NumeroHabitacionNueva, resource.Motivo, usuarioId);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new { message = result.Message });
    }
}