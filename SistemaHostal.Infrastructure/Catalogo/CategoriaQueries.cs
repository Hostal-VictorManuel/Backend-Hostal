using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Catalogo;

public class CategoriaQueries(SistemaHostalDbContext context) : ICategoriaQueries
{
    public async Task<IReadOnlyList<CategoriaResumenDto>> BuscarAsync(string? texto, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Categoria>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(texto))
            query = query.Where(c => c.Nombre.Contains(texto));

        return await query
            .Select(c => new CategoriaResumenDto(
                c.Id,
                c.Nombre,
                c.Estado.ToString(),
                context.Set<Domain.Catalogo.Producto>().Count(p => p.CategoriaId == c.Id),
                c.FechaCreacion))
            .ToListAsync(cancellationToken);
    }
}