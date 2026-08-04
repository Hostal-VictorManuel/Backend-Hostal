using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Ventas;
using SistemaHostal.Domain.Trabajadores;
using SistemaHostal.Domain.Ventas;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Ventas;

public class VentaQueries(SistemaHostalDbContext context) : IVentaQueries
{
    public async Task<IReadOnlyList<VentaResumenDto>> BuscarAsync(
        DateTime? fecha, string? numeroVenta, int? turnoId, string? estado, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Venta>().AsNoTracking().AsQueryable();

        if (fecha.HasValue)
        {
            var fechaUtc = DateTime.SpecifyKind(fecha.Value.Date, DateTimeKind.Utc);
            query = query.Where(v => v.FechaHoraInicio.Date == fechaUtc);
        }

        if (!string.IsNullOrWhiteSpace(numeroVenta))
            query = query.Where(v => v.NumeroVenta.Contains(numeroVenta));

        if (turnoId.HasValue)
            query = query.Where(v => v.TurnoId == turnoId.Value);

        if (!string.IsNullOrWhiteSpace(estado) && Enum.TryParse<EstadoVenta>(estado, out var estadoEnum))
            query = query.Where(v => v.Estado == estadoEnum);

        return await query
            .OrderByDescending(v => v.FechaHoraInicio)
            .Select(v => new VentaResumenDto(
                v.Id, v.NumeroVenta, v.TurnoId, v.NumeroHabitacion, v.TrabajadorId,
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
            venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.TrabajadorId, venta.Observaciones,
            venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(),
            venta.MotivoAnulacion, venta.UsuarioAnulacionId, string.Empty, venta.FechaHoraAnulacion,
            venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
            venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
            venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList());
    }
    
    public async Task<IReadOnlyList<HabitacionConsumoPendienteDto>> ObtenerHabitacionesConConsumosPendientesAsync(CancellationToken cancellationToken = default)
    {
        var ventas = await context.Set<Venta>().AsNoTracking()
            .Where(v => v.Estado == EstadoVenta.Pendiente && v.NumeroHabitacion != null)
            .Select(v => new { v.NumeroHabitacion, Total = v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

        return ventas
            .GroupBy(v => v.NumeroHabitacion!)
            .Select(g => new HabitacionConsumoPendienteDto(g.Key, g.Count(), g.Sum(v => v.Total)))
            .ToList();
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
            venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.TrabajadorId, venta.Observaciones,
            venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(),
            venta.MotivoAnulacion, venta.UsuarioAnulacionId, string.Empty, venta.FechaHoraAnulacion,
            venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
            venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
            venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList()
        )).ToList();
    }
    public async Task<IReadOnlyList<TrabajadorConDeudaDto>> ObtenerTrabajadoresConDeudaAsync(CancellationToken cancellationToken = default)
    {
        var ventas = await context.Set<Venta>().AsNoTracking()
            .Where(v => v.Estado == EstadoVenta.Pendiente && v.TrabajadorId != null)
            .Select(v => new { v.TrabajadorId, Total = v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

        var trabajadores = await context.Set<Trabajador>().AsNoTracking()
            .ToDictionaryAsync(t => t.Id, t => t.Nombre, cancellationToken);

        return ventas
            .GroupBy(v => v.TrabajadorId!.Value)
            .Where(g => trabajadores.ContainsKey(g.Key))
            .Select(g => new TrabajadorConDeudaDto(g.Key, trabajadores[g.Key], g.Count(), g.Sum(v => v.Total)))
            .ToList();
    }
    public async Task<IReadOnlyList<VentaDetalleDto>> ObtenerConsumosPorTrabajadorAsync(int trabajadorId, CancellationToken cancellationToken = default)
    {
        var ventas = await context.Set<Venta>()
            .AsNoTracking()
            .Include(v => v.LineasVenta)
            .Include(v => v.PagosVenta)
            .Where(v => v.TrabajadorId == trabajadorId && v.Estado == EstadoVenta.Pendiente)
            .OrderBy(v => v.FechaHoraInicio)
            .ToListAsync(cancellationToken);

        return ventas.Select(venta => new VentaDetalleDto(
            venta.Id, venta.NumeroVenta, venta.TurnoId, venta.NumeroHabitacion, venta.TrabajadorId, venta.Observaciones,
            venta.Total, venta.VueltoEfectivo, venta.Estado.ToString(),
            venta.MotivoAnulacion, venta.UsuarioAnulacionId, string.Empty, venta.FechaHoraAnulacion,
            venta.FechaHoraInicio, venta.FechaHoraFinalizacion,
            venta.LineasVenta.Select(l => new LineaVentaDto(l.Id, l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, l.Subtotal)).ToList(),
            venta.PagosVenta.Select(p => new PagoVentaDto(p.Id, p.MetodoDePagoId, p.Monto, p.ReferenciaPago)).ToList()
        )).ToList();
    }
}