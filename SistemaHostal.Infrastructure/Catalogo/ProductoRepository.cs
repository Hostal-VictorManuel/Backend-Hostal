using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Catalogo;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Catalogo;

public class ProductoRepository(SistemaHostalDbContext context) : Repository<Producto>(context), IProductoRepository
{
    public async Task<bool> ExisteCodigoBarrasAsync(string codigoBarras, int? excluirProductoId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<Producto>().Where(p => p.CodigoBarras == codigoBarras);

        if (excluirProductoId.HasValue)
            query = query.Where(p => p.Id != excluirProductoId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}