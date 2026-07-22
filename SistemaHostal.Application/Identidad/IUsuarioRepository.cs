using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default);

    Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int? excluirUsuarioId = null, CancellationToken cancellationToken = default);

    Task<bool> TieneRegistrosAsociadosAsync(int usuarioId, CancellationToken cancellationToken = default);
}