using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Pagos;

namespace SistemaHostal.Application.Pagos;

public interface IMetodoDePagoRepository : IRepository<MetodoDePago>
{
    Task<bool> ExisteNombreAsync(string nombre, int? excluirMetodoDePagoId = null, CancellationToken cancellationToken = default);
}