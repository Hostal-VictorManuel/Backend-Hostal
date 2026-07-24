using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Application.Reportes;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Ventas;
using SistemaHostal.Infrastructure.Persistence;
using SistemaHostal.Application.Auditoria;
using SistemaHostal.Domain.Pagos;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Infrastructure.Reportes;

public class ReportesQueries(
    SistemaHostalDbContext context,
    IProductoQueries productoQueries,
    IRegistroBitacoraQueries registroBitacoraQueries) : IReportesQueries
{
    private static readonly EstadoVenta[] EstadosVentaValida = [EstadoVenta.Pagada, EstadoVenta.Pendiente];
    private static DateTime ComoUtc(DateTime fecha) => DateTime.SpecifyKind(fecha, DateTimeKind.Utc);

    public async Task<DashboardRecepcionistaDto> ObtenerDashboardRecepcionistaAsync(CancellationToken cancellationToken = default)
    {
        var hoy = DateTime.UtcNow.Date;

        var ventasHoy = await context.Set<Venta>().AsNoTracking()
            .Where(v => v.FechaHoraInicio.Date == hoy && EstadosVentaValida.Contains(v.Estado))
            .Select(v => new { v.Id, Total = v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

        var productosVendidosHoy = await (
            from l in context.Set<LineaVenta>().AsNoTracking()
            join v in context.Set<Venta>().AsNoTracking() on l.VentaId equals v.Id
            where v.FechaHoraInicio.Date == hoy && EstadosVentaValida.Contains(v.Estado)
            select l.Cantidad
        ).SumAsync(cancellationToken);

        var stockCritico = await productoQueries.ObtenerConStockCriticoAsync(cancellationToken);

        return new DashboardRecepcionistaDto(
            ventasHoy.Count, ventasHoy.Sum(v => v.Total), productosVendidosHoy, stockCritico);
    }

    public async Task<DashboardAdministradorDto> ObtenerDashboardAdministradorAsync(CancellationToken cancellationToken = default)
    {
        var hoy = DateTime.UtcNow.Date;
        var inicioMes = new DateTime(hoy.Year, hoy.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var ventasHoy = await context.Set<Venta>().AsNoTracking()
            .Where(v => v.FechaHoraInicio.Date == hoy && EstadosVentaValida.Contains(v.Estado))
            .Select(v => v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad))
            .ToListAsync(cancellationToken);

        var ventasMes = await context.Set<Venta>().AsNoTracking()
            .Where(v => v.FechaHoraInicio >= inicioMes && EstadosVentaValida.Contains(v.Estado))
            .Select(v => v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad))
            .ToListAsync(cancellationToken);

        var topProductos = await ObtenerProductosMasVendidosAsync(null, null, 5, cancellationToken);
        var stockCritico = await productoQueries.ObtenerConStockCriticoAsync(cancellationToken);

        var usuariosActivos = await context.Set<Usuario>().AsNoTracking()
            .CountAsync(u => u.Estado == EstadoUsuario.Activo, cancellationToken);

        return new DashboardAdministradorDto(
            ventasHoy.Count, ventasHoy.Sum(),
            ventasMes.Count, ventasMes.Sum(),
            topProductos, stockCritico, usuariosActivos);
    }

    public async Task<IReadOnlyList<ProductoMasVendidoDto>> ObtenerProductosMasVendidosAsync(
        DateTime? desde, DateTime? hasta, int top, CancellationToken cancellationToken = default)
    {
        var lineas = await ObtenerLineasEnRangoAsync(desde, hasta, cancellationToken);

        return lineas
            .GroupBy(l => new { l.ProductoId, l.NombreProducto })
            .Select(g => new ProductoMasVendidoDto(
                g.Key.ProductoId, g.Key.NombreProducto, g.Sum(l => l.Cantidad), g.Sum(l => l.PrecioUnitario * l.Cantidad)))
            .OrderByDescending(p => p.CantidadVendida)
            .Take(top)
            .ToList();
    }

    public async Task<IReadOnlyList<ProductoMasVendidoDto>> ObtenerProductosMenorRotacionAsync(
        DateTime? desde, DateTime? hasta, int top, CancellationToken cancellationToken = default)
    {
        var lineas = await ObtenerLineasEnRangoAsync(desde, hasta, cancellationToken);

        return lineas
            .GroupBy(l => new { l.ProductoId, l.NombreProducto })
            .Select(g => new ProductoMasVendidoDto(
                g.Key.ProductoId, g.Key.NombreProducto, g.Sum(l => l.Cantidad), g.Sum(l => l.PrecioUnitario * l.Cantidad)))
            .OrderBy(p => p.CantidadVendida)
            .Take(top)
            .ToList();
    }

    private async Task<List<(int ProductoId, string NombreProducto, decimal PrecioUnitario, int Cantidad)>> ObtenerLineasEnRangoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken)
    {
        var query =
            from l in context.Set<LineaVenta>().AsNoTracking()
            join v in context.Set<Venta>().AsNoTracking() on l.VentaId equals v.Id
            where EstadosVentaValida.Contains(v.Estado)
            select new { l.ProductoId, l.NombreProducto, l.PrecioUnitario, l.Cantidad, v.FechaHoraInicio };

        if (desde.HasValue)
            query = query.Where(x => x.FechaHoraInicio >= ComoUtc(desde.Value.Date));

        if (hasta.HasValue)
            query = query.Where(x => x.FechaHoraInicio < ComoUtc(hasta.Value.Date.AddDays(1)));

        var resultado = await query.ToListAsync(cancellationToken);

        return resultado.Select(x => (x.ProductoId, x.NombreProducto, x.PrecioUnitario, x.Cantidad)).ToList();
    }
    
    public async Task<IReadOnlyList<VentasPorDiaDto>> ObtenerVentasPorPeriodoAsync(
    DateTime desde, DateTime hasta, CancellationToken cancellationToken = default)
    {
        var ventas = await context.Set<Venta>().AsNoTracking()
            .Where(v => EstadosVentaValida.Contains(v.Estado) && v.FechaHoraInicio >= ComoUtc(desde.Date) && v.FechaHoraInicio < ComoUtc(hasta.Date.AddDays(1)))
            .Select(v => new { Fecha = v.FechaHoraInicio.Date, Total = v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

    return ventas
        .GroupBy(v => v.Fecha)
        .Select(g => new VentasPorDiaDto(g.Key, g.Count(), g.Sum(v => v.Total)))
        .OrderBy(v => v.Fecha)
        .ToList();
    }

    public async Task<IReadOnlyList<VentasPorMetodoPagoDto>> ObtenerVentasPorMetodoPagoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default)
    {
        var query =
            from p in context.Set<PagoVenta>().AsNoTracking()
            join v in context.Set<Venta>().AsNoTracking() on p.VentaId equals v.Id
            join m in context.Set<MetodoDePago>().AsNoTracking() on p.MetodoDePagoId equals m.Id
            where EstadosVentaValida.Contains(v.Estado)
            select new { Pago = p, Venta = v, Metodo = m };

        if (desde.HasValue)
            query = query.Where(x => x.Venta.FechaHoraInicio >= ComoUtc(desde.Value.Date));

        if (hasta.HasValue)
            query = query.Where(x => x.Venta.FechaHoraInicio < ComoUtc(hasta.Value.Date.AddDays(1)));
        
        return await query
            .GroupBy(x => new { x.Metodo.Id, x.Metodo.Nombre })
            .Select(g => new VentasPorMetodoPagoDto(g.Key.Id, g.Key.Nombre, g.Sum(x => x.Pago.Monto)))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RegistroBitacoraDto>> ObtenerUltimosMovimientosAsync(
        int cantidad, CancellationToken cancellationToken = default)
    {
        var registros = await registroBitacoraQueries.BuscarAsync(null, null, null, null, cancellationToken);
        return registros.Take(cantidad).ToList();
    }

    public async Task<FlujoCajaMensualDto> ObtenerFlujoCajaMensualAsync(
        int anio, int mes, CancellationToken cancellationToken = default)
    {
        if (mes < 1 || mes > 12)
            throw new ArgumentException("El mes debe estar entre 1 y 12.", nameof(mes));

        var inicio = DateTime.SpecifyKind(new DateTime(anio, mes, 1), DateTimeKind.Utc);
        var fin = inicio.AddMonths(1);

        var ventas = await context.Set<Venta>().AsNoTracking()
            .Where(v => EstadosVentaValida.Contains(v.Estado) && v.FechaHoraInicio >= inicio && v.FechaHoraInicio < fin)
            .Select(v => new { Fecha = v.FechaHoraInicio.Date, Total = v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

        var ingresosPorDia = ventas
            .GroupBy(v => v.Fecha)
            .Select(g => new IngresoDiarioDto(g.Key, g.Sum(v => v.Total)))
            .OrderBy(i => i.Fecha)
            .ToList();

        return new FlujoCajaMensualDto(anio, mes, ventas.Sum(v => v.Total), ingresosPorDia);
    }

    public async Task<IReadOnlyList<IngresoPorTurnoDto>> ObtenerIngresosPorTurnoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default)
    {
        var queryTurnos =
            from t in context.Set<Turno>().AsNoTracking()
            join u in context.Set<Usuario>().AsNoTracking() on t.UsuarioId equals u.Id
            select new { Turno = t, Usuario = u };

        if (desde.HasValue)
            queryTurnos = queryTurnos.Where(x => x.Turno.FechaHoraInicio >= ComoUtc(desde.Value.Date));

        if (hasta.HasValue)
            queryTurnos = queryTurnos.Where(x => x.Turno.FechaHoraInicio < ComoUtc(hasta.Value.Date.AddDays(1)));
        
        var turnos = await queryTurnos.ToListAsync(cancellationToken);

        var totalesPorTurno = await context.Set<Venta>().AsNoTracking()
            .Where(v => EstadosVentaValida.Contains(v.Estado))
            .Select(v => new { v.TurnoId, Total = v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

        var totalesDict = totalesPorTurno
            .GroupBy(v => v.TurnoId)
            .ToDictionary(g => g.Key, g => g.Sum(v => v.Total));

        return turnos.Select(x => new IngresoPorTurnoDto(
            x.Turno.Id, x.Usuario.NombreCompleto, x.Turno.FechaHoraInicio, x.Turno.FechaHoraFin,
            totalesDict.GetValueOrDefault(x.Turno.Id, 0m))).ToList();
    }

    public async Task<IReadOnlyList<VentasPorRecepcionistaDto>> ObtenerVentasPorRecepcionistaAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default)
    {
        var query =
            from v in context.Set<Venta>().AsNoTracking()
            join t in context.Set<Turno>().AsNoTracking() on v.TurnoId equals t.Id
            join u in context.Set<Usuario>().AsNoTracking() on t.UsuarioId equals u.Id
            where EstadosVentaValida.Contains(v.Estado)
            select new { Venta = v, Usuario = u };

        if (desde.HasValue)
            query = query.Where(x => x.Venta.FechaHoraInicio >= ComoUtc(desde.Value.Date));

        if (hasta.HasValue)
            query = query.Where(x => x.Venta.FechaHoraInicio < ComoUtc(hasta.Value.Date.AddDays(1)));
        
        var ventas = await query
            .Select(x => new { x.Usuario.Id, x.Usuario.NombreCompleto, Total = x.Venta.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad) })
            .ToListAsync(cancellationToken);

        return ventas
            .GroupBy(x => new { x.Id, x.NombreCompleto })
            .Select(g => new VentasPorRecepcionistaDto(g.Key.Id, g.Key.NombreCompleto, g.Count(), g.Sum(x => x.Total)))
            .ToList();
    }

    public async Task<IReadOnlyList<VentaPagoMixtoDto>> ObtenerVentasConPagoMixtoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Venta>().AsNoTracking()
            .Where(v => EstadosVentaValida.Contains(v.Estado) && v.PagosVenta.Count > 1);

        if (desde.HasValue)
            query = query.Where(v => v.FechaHoraInicio >= ComoUtc(desde.Value.Date));

        if (hasta.HasValue)
            query = query.Where(v => v.FechaHoraInicio < ComoUtc(hasta.Value.Date.AddDays(1)));
        
        return await query
            .Select(v => new VentaPagoMixtoDto(v.Id, v.NumeroVenta, v.PagosVenta.Count, v.LineasVenta.Sum(l => l.PrecioUnitario * l.Cantidad)))
            .ToListAsync(cancellationToken);
    }
    
}