using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Auditoria;

public class ProductoModificadoHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<ProductoModificado>
{
    public async Task Handle(ProductoModificado notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Catalogo, TipoOperacionAuditoria.Modificacion,
            $"Se modificó el producto \"{notification.NombreProducto}\" (Id: {notification.ProductoId}). " +
            $"Precio anterior: {notification.PrecioAnterior:F2}, precio nuevo: {notification.PrecioNuevo:F2}.");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}