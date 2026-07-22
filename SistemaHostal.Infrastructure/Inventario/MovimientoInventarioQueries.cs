using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Inventario;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Inventario;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Inventario;

public class MovimientoInventarioQueries(SistemaHostalDbContext context) : IMovimientoInventarioQueries
{
    public async Task<IReadOnlyList<MovimientoInventarioDto>> ListarAsync(
        int? productoId, DateTime? fecha, CancellationToken cancellationToken = default)
    {
        var query =
            from m in context.Set<MovimientoInventario>().AsNoTracking()
            join p in context.Set<Producto>().AsNoTracking() on m.ProductoId equals p.Id
            join u in context.Set<Usuario>().AsNoTracking() on m.UsuarioId equals u.Id into usuarios
            from u in usuarios.DefaultIfEmpty()
            select new { Movimiento = m, Producto = p, Usuario = u };

        if (productoId.HasValue)
            query = query.Where(x => x.Movimiento.ProductoId == productoId.Value);

        if (fecha.HasValue)
            query = query.Where(x => x.Movimiento.FechaHora.Date == fecha.Value.Date);

        return await query
            .OrderByDescending(x => x.Movimiento.FechaHora)
            .Select(x => new MovimientoInventarioDto(
                x.Movimiento.Id, x.Movimiento.ProductoId, x.Producto.Nombre, x.Movimiento.Tipo.ToString(),
                x.Movimiento.Cantidad, x.Movimiento.Motivo, x.Movimiento.VentaId, x.Movimiento.UsuarioId,
                x.Usuario != null ? x.Usuario.NombreCompleto : string.Empty, x.Movimiento.FechaHora))
            .ToListAsync(cancellationToken);
    }
}