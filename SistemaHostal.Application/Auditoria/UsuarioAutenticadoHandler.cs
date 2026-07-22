using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Auditoria;

public class UsuarioAutenticadoHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<UsuarioAutenticado>
{
    public async Task Handle(UsuarioAutenticado notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, notification.NombreCompleto,
            ModuloAuditoria.Identidad, TipoOperacionAuditoria.Login, "Inicio de sesión exitoso.");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}