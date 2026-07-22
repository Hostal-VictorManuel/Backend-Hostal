using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Ventas;
using SistemaHostal.Domain.Ventas;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Ventas;

public class NumeroVentaGenerator(SistemaHostalDbContext context) : INumeroVentaGenerator
{
    public async Task<string> GenerarAsync(CancellationToken cancellationToken = default)
    {
        var hoy = DateTime.UtcNow;
        var prefijo = $"V{hoy:yyyyMMdd}-";

        var ultimoNumeroHoy = await context.Set<Venta>()
            .Where(v => v.NumeroVenta.StartsWith(prefijo))
            .OrderByDescending(v => v.NumeroVenta)
            .Select(v => v.NumeroVenta)
            .FirstOrDefaultAsync(cancellationToken);

        var siguienteSecuencial = 1;
        if (ultimoNumeroHoy is not null && int.TryParse(ultimoNumeroHoy.AsSpan(prefijo.Length), out var ultimo))
            siguienteSecuencial = ultimo + 1;

        return $"{prefijo}{siguienteSecuencial:D4}";
    }
}