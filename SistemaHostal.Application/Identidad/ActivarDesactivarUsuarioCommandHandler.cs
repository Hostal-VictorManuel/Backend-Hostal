using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public class ActivarDesactivarUsuarioCommandHandler(
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ActivarDesactivarUsuarioCommand, Result>
{
    public async Task<Result> Handle(ActivarDesactivarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
            return Result.Failure(IdentidadError.UsuarioNoEncontrado, "Usuario no encontrado.");

        if (request.Activar) usuario.Activar();
        else usuario.Desactivar();

        usuarioRepository.Update(usuario);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}