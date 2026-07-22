using Microsoft.EntityFrameworkCore;
using SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Identidad;

public class UsuarioQueries(SistemaHostalDbContext context) : IUsuarioQueries
{
    public async Task<IReadOnlyList<UsuarioResumenDto>> BuscarAsync(
        string? texto, RolUsuario? rol, EstadoUsuario? estado, CancellationToken cancellationToken = default)
    {
        var query = context.Usuarios.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(texto))
            query = query.Where(u => u.NombreCompleto.Contains(texto) || u.NombreUsuario.Contains(texto));

        if (rol.HasValue)
            query = query.Where(u => u.Rol == rol.Value);

        if (estado.HasValue)
            query = query.Where(u => u.Estado == estado.Value);

        return await query
            .Select(u => new UsuarioResumenDto(
                u.Id, u.NombreCompleto, u.NombreUsuario, u.Rol.ToString(), u.Estado.ToString(), u.UltimoAcceso))
            .ToListAsync(cancellationToken);
    }

    public async Task<UsuarioDetalleDto?> ObtenerDetalleAsync(int usuarioId, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.AsNoTracking()
            .Where(u => u.Id == usuarioId)
            .Select(u => new UsuarioDetalleDto(
                u.Id, u.NombreCompleto, u.NombreUsuario, u.Rol.ToString(), u.Estado.ToString(), u.FechaCreacion, u.UltimoAcceso))
            .FirstOrDefaultAsync(cancellationToken);
    }
}