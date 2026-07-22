using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Application.Auditoria;

public class ProductoRegistradoHandler(
    IRegistroBitacoraRepository bitacoraRepository,
    IUnitOfWork unitOfWork) : INotificationHandler<ProductoRegistrado>
{
    public async Task Handle(ProductoRegistrado notification, CancellationToken cancellationToken)
    {
        var registro = new RegistroBitacora(
            notification.UsuarioId, string.Empty,
            ModuloAuditoria.Catalogo, TipoOperacionAuditoria.Creacion,
            $"Se creó el producto \"{notification.NombreProducto}\" (Id: {notification.ProductoId}).");

        await bitacoraRepository.AddAsync(registro, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}