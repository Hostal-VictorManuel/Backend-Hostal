using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Catalogo;

public class CategoriaRepository(SistemaHostalDbContext context) : Repository<Categoria>(context), ICategoriaRepository
{
    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirCategoriaId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Categoria>().Where(c => c.Nombre == nombre);

        if (excluirCategoriaId.HasValue)
            query = query.Where(c => c.Id != excluirCategoriaId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> TieneProductosAsociadosAsync(int categoriaId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Domain.Catalogo.Producto>().AnyAsync(p => p.CategoriaId == categoriaId, cancellationToken);
    }
}