using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Trabajadores;

namespace SistemaHostal.Application.Trabajadores;

public class RegistrarTrabajadorCommandHandler(
    ITrabajadorRepository trabajadorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RegistrarTrabajadorCommand, Result<TrabajadorDto>>
{
    public async Task<Result<TrabajadorDto>> Handle(RegistrarTrabajadorCommand request, CancellationToken cancellationToken)
    {
        var existe = await trabajadorRepository.ExisteNombreAsync(request.Nombre, cancellationToken: cancellationToken);
        if (existe)
            return Result<TrabajadorDto>.Failure(TrabajadoresError.NombreTrabajadorNoDisponible, "Ya existe un trabajador con ese nombre.");

        var trabajador = new Trabajador(request.Nombre);
        await trabajadorRepository.AddAsync(trabajador, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new TrabajadorDto(trabajador.Id, trabajador.Nombre, trabajador.Estado.ToString(), trabajador.FechaCreacion, trabajador.FechaModificacion);
        return Result<TrabajadorDto>.Success(dto);
    }
}