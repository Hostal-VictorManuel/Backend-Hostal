using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHostal.Application.Auditoria;
using SistemaHostal.Domain.Auditoria;

namespace SistemaHostal.API.Controllers;

[ApiController]
[Route("api/registros-bitacora")]
[Authorize(Roles = "Administrador")]
public class RegistrosBitacoraController(IRegistroBitacoraQueries registroBitacoraQueries) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Buscar(
        [FromQuery] int? usuarioId,
        [FromQuery] DateTime? fecha,
        [FromQuery] TipoOperacionAuditoria? tipoOperacion,
        [FromQuery] ModuloAuditoria? modulo,
        CancellationToken cancellationToken)
    {
        var registros = await registroBitacoraQueries.BuscarAsync(usuarioId, fecha, tipoOperacion, modulo, cancellationToken);
        return Ok(registros);
    }
}