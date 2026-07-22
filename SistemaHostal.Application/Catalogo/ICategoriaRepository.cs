using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Catalogo;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<bool> ExisteNombreAsync(string nombre, int? excluirCategoriaId = null, CancellationToken cancellationToken = default);

    Task<bool> TieneProductosAsociadosAsync(int categoriaId, CancellationToken cancellationToken = default);
}