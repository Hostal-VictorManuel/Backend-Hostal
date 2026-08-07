using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Auditoria;

public class VentaTransferidaAuditoriaHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<VentaTransferida>
{
    public async Task Handle(VentaTransferida notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Ventas, TipoOperacionAuditoria.VentaTransferida,
            $"Se transfirió la venta Id {notification.VentaId} de la habitación {notification.HabitacionAnterior} a la {notification.HabitacionNueva}. Motivo: {notification.Motivo}.");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}