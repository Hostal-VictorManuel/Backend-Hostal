using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Auditoria;

public class VentaAnuladaAuditoriaHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<VentaAnulada>
{
    public async Task Handle(VentaAnulada notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Ventas, TipoOperacionAuditoria.VentaAnulada,
            $"Se anuló la venta Id {notification.VentaId}. Motivo: {notification.Motivo}.");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}