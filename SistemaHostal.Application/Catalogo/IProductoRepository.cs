using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public interface IProductoRepository : IRepository<Producto>
{
    Task<bool> ExisteCodigoBarrasAsync(string codigoBarras, int? excluirProductoId = null, CancellationToken cancellationToken = default);
}