using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public class RestablecerPasswordCommandHandler(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<RestablecerPasswordCommand, Result>
{
    public async Task<Result> Handle(RestablecerPasswordCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(request.UsuarioId, cancellationToken);
        if (usuario is null)
            return Result.Failure(IdentidadError.UsuarioNoEncontrado, "Usuario no encontrado.");

        var nuevoHash = passwordHasher.Hash(request.NuevaPassword);
        usuario.RestablecerPassword(nuevoHash);
        usuarioRepository.Update(usuario);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO (HU-012): cuando exista el BC de Auditoría, registrar aquí el cambio de contraseña en la bitácora.
        return Result.Success();
    }
}