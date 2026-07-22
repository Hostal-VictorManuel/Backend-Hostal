using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Auditoria;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Auditoria;

public class RegistroBitacoraQueries(SistemaHostalDbContext context) : IRegistroBitacoraQueries
{
    public async Task<IReadOnlyList<RegistroBitacoraDto>> BuscarAsync(
        int? usuarioId, DateTime? fecha, TipoOperacionAuditoria? tipoOperacion, ModuloAuditoria? modulo,
        CancellationToken cancellationToken = default)
    {
        var query =
            from r in context.Set<RegistroBitacora>().AsNoTracking()
            join u in context.Set<Usuario>().AsNoTracking() on r.UsuarioId equals u.Id into usuarios
            from u in usuarios.DefaultIfEmpty()
            select new { Registro = r, Usuario = u };

        if (usuarioId.HasValue)
            query = query.Where(x => x.Registro.UsuarioId == usuarioId.Value);

        if (fecha.HasValue)
            query = query.Where(x => x.Registro.FechaHora.Date == fecha.Value.Date);

        if (tipoOperacion.HasValue)
            query = query.Where(x => x.Registro.TipoOperacion == tipoOperacion.Value);

        if (modulo.HasValue)
            query = query.Where(x => x.Registro.Modulo == modulo.Value);

        return await query
            .OrderByDescending(x => x.Registro.FechaHora)
            .Select(x => new RegistroBitacoraDto(
                x.Registro.Id, x.Registro.UsuarioId,
                x.Usuario != null ? x.Usuario.NombreCompleto : x.Registro.NombreUsuario,
                x.Registro.Modulo.ToString(), x.Registro.TipoOperacion.ToString(), x.Registro.Detalle, x.Registro.FechaHora))
            .ToListAsync(cancellationToken);
    }
}