using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Trabajadores;
using SistemaHostal.Domain.Trabajadores;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Trabajadores;

public class TrabajadorQueries(SistemaHostalDbContext context) : ITrabajadorQueries
{
    public async Task<IReadOnlyList<TrabajadorDto>> BuscarAsync(string? texto, CancellationToken cancellationToken = default)
    {
        var query = context.Set<Trabajador>().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(texto))
            query = query.Where(t => EF.Functions.ILike(t.Nombre, $"%{texto}%"));

        return await query
            .Select(t => new TrabajadorDto(t.Id, t.Nombre, t.Estado.ToString(), t.FechaCreacion, t.FechaModificacion))
            .ToListAsync(cancellationToken);
    }
}