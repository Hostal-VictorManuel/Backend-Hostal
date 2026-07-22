using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Auditoria;

public class VentaFinalizadaAuditoriaHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<VentaFinalizada>
{
    public async Task Handle(VentaFinalizada notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Ventas, TipoOperacionAuditoria.VentaRegistrada,
            $"Se registró la venta Id {notification.VentaId} con {notification.Lineas.Count} línea(s).");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}