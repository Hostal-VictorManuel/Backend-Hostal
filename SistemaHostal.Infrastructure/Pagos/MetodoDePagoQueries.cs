using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Pagos;
using SistemaHostal.Domain.Pagos;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Pagos;

public class MetodoDePagoQueries(SistemaHostalDbContext context) : IMetodoDePagoQueries
{
    public async Task<IReadOnlyList<MetodoDePagoDto>> BuscarAsync(string? texto, CancellationToken cancellationToken = default)
    {
        var query = context.Set<MetodoDePago>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(texto))
            query = query.Where(m => m.Nombre.Contains(texto));

        return await query
            .Select(m => new MetodoDePagoDto(m.Id, m.Nombre, m.Estado.ToString(), m.FechaCreacion, m.FechaModificacion))
            .ToListAsync(cancellationToken);
    }
}