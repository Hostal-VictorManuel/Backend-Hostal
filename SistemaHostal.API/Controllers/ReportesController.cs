using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.Application.Reportes;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/reportes")]
[Authorize]
public class ReportesController(IReportesQueries reportesQueries) : ControllerBase
{
    [HttpGet("dashboard/recepcionista")]
    public async Task<IActionResult> DashboardRecepcionista(CancellationToken cancellationToken)
    {
        var dashboard = await reportesQueries.ObtenerDashboardRecepcionistaAsync(cancellationToken);
        return Ok(dashboard);
    }

    [HttpGet("dashboard/administrador")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DashboardAdministrador(CancellationToken cancellationToken)
    {
        var dashboard = await reportesQueries.ObtenerDashboardAdministradorAsync(cancellationToken);
        return Ok(dashboard);
    }

    [HttpGet("productos-mas-vendidos")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ProductosMasVendidos(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, [FromQuery] int top = 10, CancellationToken cancellationToken = default)
    {
        var productos = await reportesQueries.ObtenerProductosMasVendidosAsync(desde, hasta, top, cancellationToken);
        return Ok(productos);
    }

    [HttpGet("productos-menor-rotacion")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ProductosMenorRotacion(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, [FromQuery] int top = 10, CancellationToken cancellationToken = default)
    {
        var productos = await reportesQueries.ObtenerProductosMenorRotacionAsync(desde, hasta, top, cancellationToken);
        return Ok(productos);
    }

    [HttpGet("ventas-por-periodo")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> VentasPorPeriodo(
        [FromQuery] DateTime desde, [FromQuery] DateTime hasta, CancellationToken cancellationToken)
    {
        var ventas = await reportesQueries.ObtenerVentasPorPeriodoAsync(desde, hasta, cancellationToken);
        return Ok(ventas);
    }

    [HttpGet("ventas-por-metodo-pago")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> VentasPorMetodoPago(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, CancellationToken cancellationToken)
    {
        var ventas = await reportesQueries.ObtenerVentasPorMetodoPagoAsync(desde, hasta, cancellationToken);
        return Ok(ventas);
    }

    [HttpGet("ultimos-movimientos")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UltimosMovimientos([FromQuery] int cantidad = 20, CancellationToken cancellationToken = default)
    {
        var movimientos = await reportesQueries.ObtenerUltimosMovimientosAsync(cantidad, cancellationToken);
        return Ok(movimientos);
    }

    [HttpGet("flujo-caja")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> FlujoCajaMensual([FromQuery] int anio, [FromQuery] int mes, CancellationToken cancellationToken)
    {
        var flujo = await reportesQueries.ObtenerFlujoCajaMensualAsync(anio, mes, cancellationToken);
        return Ok(flujo);
    }

    [HttpGet("ingresos-por-turno")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> IngresosPorTurno(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, CancellationToken cancellationToken)
    {
        var ingresos = await reportesQueries.ObtenerIngresosPorTurnoAsync(desde, hasta, cancellationToken);
        return Ok(ingresos);
    }

    [HttpGet("ventas-por-recepcionista")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> VentasPorRecepcionista(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, CancellationToken cancellationToken)
    {
        var ventas = await reportesQueries.ObtenerVentasPorRecepcionistaAsync(desde, hasta, cancellationToken);
        return Ok(ventas);
    }

    [HttpGet("ventas-pago-mixto")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> VentasConPagoMixto(
        [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta, CancellationToken cancellationToken)
    {
        var ventas = await reportesQueries.ObtenerVentasConPagoMixtoAsync(desde, hasta, cancellationToken);
        return Ok(ventas);
    }
}