using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Application.Auditoria;

public class UsuarioDesconectadoHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<UsuarioDesconectado>
{
    public async Task Handle(UsuarioDesconectado notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, notification.NombreCompleto,
            ModuloAuditoria.Identidad, TipoOperacionAuditoria.Logout, "Cierre de sesión.");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}