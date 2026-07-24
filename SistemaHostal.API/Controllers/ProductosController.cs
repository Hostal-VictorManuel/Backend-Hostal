using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SistemaHostal.API.Resources.Catalogo;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/productos")]
[Authorize]
public class ProductosController(IMediator mediator, IProductoQueries productoQueries) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Registrar(RegistrarProductoResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var command = new RegistrarProductoCommand(
            resource.CodigoBarras, resource.Nombre, resource.CategoriaId,
            resource.Precio, resource.StockInicial, resource.StockMinimo, resource.ImagenUrl, usuarioId);

        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(ObtenerDetalle), new { id = result.Value!.Id }, result.Value)
            : Conflict(new { message = result.Message });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id, EditarProductoResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var command = new EditarProductoCommand(id, resource.Nombre, resource.CategoriaId, resource.Precio, resource.StockMinimo, resource.ImagenUrl, usuarioId);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/activar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Activar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarProductoCommand(id, true), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/desactivar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Desactivar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarProductoCommand(id, false), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/ingreso-stock")]
    public async Task<IActionResult> RegistrarIngresoStock(int id, IngresoStockResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var result = await mediator.Send(new SistemaHostal.Application.Inventario.RegistrarIngresoCommand(id, resource.Cantidad, usuarioId), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/ajustar-stock")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> AjustarStock(int id, AjustarStockResource resource, CancellationToken cancellationToken)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);
        var result = await mediator.Send(new SistemaHostal.Application.Inventario.RegistrarAjusteCommand(id, resource.NuevoStock, resource.Motivo, usuarioId), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Buscar(
        [FromQuery] string? texto,
        [FromQuery] int? categoriaId,
        [FromQuery] EstadoProducto? estado,
        CancellationToken cancellationToken)
    {
        var productos = await productoQueries.BuscarAsync(texto, categoriaId, estado, cancellationToken);
        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerDetalle(int id, CancellationToken cancellationToken)
    {
        var producto = await productoQueries.ObtenerDetalleAsync(id, cancellationToken);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpGet("stock-critico")]
    public async Task<IActionResult> ObtenerConStockCritico(CancellationToken cancellationToken)
    {
        var productos = await productoQueries.ObtenerConStockCriticoAsync(cancellationToken);
        return Ok(productos);
    }
}