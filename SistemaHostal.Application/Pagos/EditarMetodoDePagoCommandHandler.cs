using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Pagos;

namespace SistemaHostal.Application.Pagos;

public class EditarMetodoDePagoCommandHandler(
    IMetodoDePagoRepository metodoDePagoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditarMetodoDePagoCommand, Result<MetodoDePagoDto>>
{
    public async Task<Result<MetodoDePagoDto>> Handle(EditarMetodoDePagoCommand request, CancellationToken cancellationToken)
    {
        var metodo = await metodoDePagoRepository.GetByIdAsync(request.MetodoDePagoId, cancellationToken);
        if (metodo is null)
            return Result<MetodoDePagoDto>.Failure(PagosError.MetodoDePagoNoEncontrado, "Método de pago no encontrado.");

        var nombreEnUso = await metodoDePagoRepository.ExisteNombreAsync(request.Nombre, request.MetodoDePagoId, cancellationToken);
        if (nombreEnUso)
            return Result<MetodoDePagoDto>.Failure(PagosError.NombreMetodoDePagoNoDisponible, "Ya existe un método de pago con ese nombre.");

        metodo.Editar(request.Nombre);
        metodoDePagoRepository.Update(metodo);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new MetodoDePagoDto(metodo.Id, metodo.Nombre, metodo.Estado.ToString(), metodo.FechaCreacion, metodo.FechaModificacion);
        return Result<MetodoDePagoDto>.Success(dto);
    }
}