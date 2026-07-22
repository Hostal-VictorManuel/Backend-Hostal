namespace SistemaHostal.Application.Identidad;
using SistemaHostal.Domain.Identidad;

public interface IUsuarioQueries
{
    Task<IReadOnlyList<UsuarioResumenDto>> BuscarAsync(
        string? texto,
        RolUsuario? rol,
        EstadoUsuario? estado,
        CancellationToken cancellationToken = default);

    Task<UsuarioDetalleDto?> ObtenerDetalleAsync(int usuarioId, CancellationToken cancellationToken = default);
}