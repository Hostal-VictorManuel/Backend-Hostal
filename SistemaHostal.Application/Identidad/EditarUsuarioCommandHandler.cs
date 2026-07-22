using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public class EditarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditarUsuarioCommand, Result<UsuarioResumenDto>>
{
    public async Task<Result<UsuarioResumenDto>> Handle(EditarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
            return Result<UsuarioResumenDto>.Failure(IdentidadError.UsuarioNoEncontrado, "Usuario no encontrado.");

        var nombreEnUso = await usuarioRepository.ExisteNombreUsuarioAsync(request.NombreUsuario, request.UsuarioId, cancellationToken);
        if (nombreEnUso)
            return Result<UsuarioResumenDto>.Failure(IdentidadError.NombreUsuarioNoDisponible, "El nombre de usuario ya está en uso.");

        usuario.Editar(request.NombreCompleto, request.NombreUsuario, request.Rol);
        usuarioRepository.Update(usuario);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new UsuarioResumenDto(
            usuario.Id, usuario.NombreCompleto, usuario.NombreUsuario,
            usuario.Rol.ToString(), usuario.Estado.ToString(), usuario.UltimoAcceso);

        return Result<UsuarioResumenDto>.Success(dto);
    }
}