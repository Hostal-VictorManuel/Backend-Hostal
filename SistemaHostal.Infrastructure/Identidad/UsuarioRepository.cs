using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Identidad;

public class UsuarioRepository(SistemaHostalDbContext context) : Repository<Usuario>(context), IUsuarioRepository
{
    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default)
        => await Context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario, cancellationToken);

    public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, int? excluirUsuarioId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.Usuarios.Where(u => u.NombreUsuario == nombreUsuario);

        if (excluirUsuarioId.HasValue)
            query = query.Where(u => u.Id != excluirUsuarioId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public Task<bool> TieneRegistrosAsociadosAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        // TODO (HU-016): cuando existan Ventas/Turnos/Auditoría, consultar aquí si el usuario tiene
        // registros asociados antes de permitir eliminación física. Todavía no hay otros BC implementados.
        return Task.FromResult(false);
    }
}