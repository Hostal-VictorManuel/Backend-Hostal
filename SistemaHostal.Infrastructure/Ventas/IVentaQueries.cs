using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Ventas;
using SistemaHostal.Domain.Ventas;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Ventas;

public class VentaQueries(SistemaHostalDbContext context) : IVentaQueries
{
    public async Task<IReadOnlyList<VentaResumenDto>> BuscarAsync(
        DateTime? fecha, string? numeroVenta, int? turnoId, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Venta>().AsNoTracking().AsQueryable();

        if (fecha.HasValue)
            query = query.Where(v => v.FechaHoraInicio.Date == fecha.Value.Date);

        if (!string.IsNullOrWhiteSpace(numeroVenta))
            query = query.Where(v => v.NumeroVenta.Contains(numeroVenta));

        if (turnoId.HasValue)
            query = query.Where(v => v.TurnoId == turnoId.Value);

        return await query
            .OrderByDescending(v => v.FechaHoraInicio)
            .Select(v => new VentaResumenDto(
                v.Id, v.NumeroVenta, v.TurnoId, v.NumeroHabitacion,
                v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad), v.Estado.ToString(), v.FechaHoraInicio, v.FechaHoraFinalizacion))
            .ToListAsync(cancellationToken);
    }

    public async Task<VentaDetalleDto?> ObtenerDetalleAsync(int ventaId, CancellationToken cancellationToken = default)
    {
        var venta = await context.Set<Venta>()
            .AsNoTracking()
            .Include(v => v.LineasVenta)
            .Include(v => v.PagosVenta)
            .FirstOrDefaultAsync(v => v.Id == ventaId, cancellationToken);

        if (venta is null) return null;

        return new VentaDetalleDto(
            venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.Observaciones,
            venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(), venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
            venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
            venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList());
    }
    
    public async Task<IReadOnlyList<HabitacionConsumoPendienteDto>> ObtenerHabitacionesConConsumosPendientesAsync(CancellationToken cancellationToken = default)
    {
        var query =
            from v in context.Set<Venta>().AsNoTracking()
            where v.Estado == EstadoVenta.Pendiente && v.NumeroHabitacion != null
            group v by v.NumeroHabitacion into g
            select new HabitacionConsumoPendienteDto(
                g.Key!,
                g.Count(),
                g.Sum(v => v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad)));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VentaDetalleDto>> ObtenerConsumosPorHabitacionAsync(string numeroHabitacion, CancellationToken cancellationToken = default)
    {
        var ventas = await context.Set<Venta>()
            .AsNoTracking()
            .Include(v => v.LineasVenta)
            .Include(v => v.PagosVenta)
            .Where(v => v.NumeroHabitacion == numeroHabitacion && v.Estado == EstadoVenta.Pendiente)
            .OrderBy(v => v.FechaHoraInicio)
            .ToListAsync(cancellationToken);

        return ventas.Select(venta => new VentaDetalleDto(
            venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.Observaciones,
            venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(), venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
            venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
            venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList()
        )).ToList();
    }
}