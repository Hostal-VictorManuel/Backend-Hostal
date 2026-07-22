using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Identidad;

public class LoginCommandHandler(
    IUsuarioRepository usuarioRepository,
    IIntentoAccesoRepository intentoAccesoRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork,
    IPublisher publisher) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.ObtenerPorNombreUsuarioAsync(request.NombreUsuario, cancellationToken);

        if (usuario is null || !passwordHasher.Verificar(request.Password, usuario.PasswordHash))
        {
            await intentoAccesoRepository.AddAsync(
                new IntentoAcceso(request.NombreUsuario, request.Ip, ResultadoAcceso.Fallido), cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<LoginResponseDto>.Failure(IdentidadError.CredencialesInvalidas, "Usuario o contraseña incorrectos.");
        }

        if (!usuario.PuedeIniciarSesion())
        {
            await intentoAccesoRepository.AddAsync(
                new IntentoAcceso(request.NombreUsuario, request.Ip, ResultadoAcceso.Fallido), cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<LoginResponseDto>.Failure(IdentidadError.UsuarioDesactivado, "Usuario deshabilitado. Contacte al administrador.");
        }

        usuario.RegistrarAcceso();
        usuarioRepository.Update(usuario);

        await intentoAccesoRepository.AddAsync(
            new IntentoAcceso(request.NombreUsuario, request.Ip, ResultadoAcceso.Exitoso), cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var token = jwtService.GenerarToken(usuario.Id, usuario.NombreCompleto, usuario.Rol);

        await publisher.Publish(new UsuarioAutenticado(usuario.Id, usuario.NombreCompleto), cancellationToken);

        return Result<LoginResponseDto>.Success(new LoginResponseDto(token, usuario.NombreCompleto, usuario.Rol.ToString()));
    }
}