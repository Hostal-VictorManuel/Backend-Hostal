using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Pagos;
using SistemaHostal.Domain.Pagos;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Pagos;

public class MetodoDePagoRepository(SistemaHostalDbContext context) : Repository<MetodoDePago>(context), IMetodoDePagoRepository
{
    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirMetodoDePagoId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<MetodoDePago>().Where(m => m.Nombre == nombre);

        if (excluirMetodoDePagoId.HasValue)
            query = query.Where(m => m.Id != excluirMetodoDePagoId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}