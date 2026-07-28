using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Trabajadores;
using SistemaHostal.Domain.Trabajadores;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Trabajadores;

public class TrabajadorRepository(SistemaHostalDbContext context) : Repository<Trabajador>(context), ITrabajadorRepository
{
    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirTrabajadorId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Trabajador>().Where(t => t.Nombre == nombre);

        if (excluirTrabajadorId.HasValue)
            query = query.Where(t => t.Id != excluirTrabajadorId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}