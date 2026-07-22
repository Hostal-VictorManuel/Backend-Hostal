using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Pagos;

namespace SistemaHostal.Application.Pagos;

public class RegistrarMetodoDePagoCommandHandler(
    IMetodoDePagoRepository metodoDePagoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarMetodoDePagoCommand, Result<MetodoDePagoDto>>
{
    public async Task<Result<MetodoDePagoDto>> Handle(RegistrarMetodoDePagoCommand request, CancellationToken cancellationToken)
    {
        var existe = await metodoDePagoRepository.ExisteNombreAsync(request.Nombre, cancellationToken: cancellationToken);
        if (existe)
            return Result<MetodoDePagoDto>.Failure(PagosError.NombreMetodoDePagoNoDisponible, "Ya existe un método de pago con ese nombre.");

        var metodo = new MetodoDePago(request.Nombre);
        await metodoDePagoRepository.AddAsync(metodo, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new MetodoDePagoDto(metodo.Id, metodo.Nombre, metodo.Estado.ToString(), metodo.FechaCreacion, metodo.FechaModificacion);
        return Result<MetodoDePagoDto>.Success(dto);
    }
}