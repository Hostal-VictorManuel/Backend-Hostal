using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Ventas;
using SistemaHostal.Domain.Ventas;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Ventas;

public class VentaRepository(SistemaHostalDbContext context) : Repository<Venta>(context), IVentaRepository
{
    public async Task<Venta?> ObtenerConLineasYPagosAsync(int ventaId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Venta>()
            .Include(v => v.LineasVenta)
            .Include(v => v.PagosVenta)
            .FirstOrDefaultAsync(v => v.Id == ventaId, cancellationToken);
    }

    public async Task<Venta?> ObtenerVentaEnProcesoPorTurnoAsync(int turnoId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Venta>()
            .Include(v => v.LineasVenta)
            .Include(v => v.PagosVenta)
            .FirstOrDefaultAsync(v => v.TurnoId == turnoId && v.Estado == EstadoVenta.EnProceso, cancellationToken);
    }
}