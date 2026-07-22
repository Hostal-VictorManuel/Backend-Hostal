using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public class RegistrarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarUsuarioCommand, Result<UsuarioResumenDto>>
{
    public async Task<Result<UsuarioResumenDto>> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var existe = await usuarioRepository.ExisteNombreUsuarioAsync(request.NombreUsuario, cancellationToken: cancellationToken);
        if (existe)
            return Result<UsuarioResumenDto>.Failure(IdentidadError.NombreUsuarioNoDisponible, "El nombre de usuario ya está en uso.");

        var passwordHash = passwordHasher.Hash(request.Password);
        var usuario = new Usuario(request.NombreCompleto, request.NombreUsuario, passwordHash, request.Rol);

        await usuarioRepository.AddAsync(usuario, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new UsuarioResumenDto(
            usuario.Id, usuario.NombreCompleto, usuario.NombreUsuario,
            usuario.Rol.ToString(), usuario.Estado.ToString(), usuario.UltimoAcceso);

        return Result<UsuarioResumenDto>.Success(dto);
    }
}