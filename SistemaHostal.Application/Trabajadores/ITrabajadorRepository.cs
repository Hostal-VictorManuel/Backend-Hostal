using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Trabajadores;

namespace SistemaHostal.Application.Trabajadores;

public interface ITrabajadorRepository : IRepository<Trabajador>
{
    Task<bool> ExisteNombreAsync(string nombre, int? excluirTrabajadorId = null, CancellationToken cancellationToken = default);
}