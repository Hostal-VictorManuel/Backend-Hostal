using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.API.Resources.Catalogo;
using SistemaHostal.Application.Catalogo;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/categorias")]
[Authorize(Roles = "Administrador")]
public class CategoriasController(IMediator mediator, ICategoriaQueries categoriaQueries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Registrar(RegistrarCategoriaCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Created(string.Empty, result.Value) : Conflict(new { message = result.Message });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Editar(int id, EditarCategoriaResource resource, CancellationToken cancellationToken)
    {
        var command = new EditarCategoriaCommand(id, resource.Nombre);
        var result = await mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/activar")]
    public async Task<IActionResult> Activar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarCategoriaCommand(id, true), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpPatch("{id:int}/desactivar")]
    public async Task<IActionResult> Desactivar(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivarDesactivarCategoriaCommand(id, false), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { message = result.Message });
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] string? texto, CancellationToken cancellationToken)
    {
        var categorias = await categoriaQueries.BuscarAsync(texto, cancellationToken);
        return Ok(categorias);
    }
}