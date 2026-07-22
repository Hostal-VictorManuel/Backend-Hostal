using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Inventario;

namespace SistemaHostal.Application.Auditoria;

public class IngresoRegistradoHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<IngresoRegistrado>
{
    public async Task Handle(IngresoRegistrado notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Inventario, TipoOperacionAuditoria.IngresoInventario,
            $"Ingreso de {notification.Cantidad} unidades de \"{notification.NombreProducto}\".");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}