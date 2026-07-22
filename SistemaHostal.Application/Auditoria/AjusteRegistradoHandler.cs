using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Inventario;

namespace SistemaHostal.Application.Auditoria;

public class AjusteRegistradoHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<AjusteRegistrado>
{
    public async Task Handle(AjusteRegistrado notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Inventario, TipoOperacionAuditoria.AjusteInventario,
            $"Ajuste de {notification.Diferencia:+#;-#;0} unidades en \"{notification.NombreProducto}\". Motivo: {notification.Motivo}.");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}