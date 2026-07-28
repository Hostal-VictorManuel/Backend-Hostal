using MediatR;
using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Trabajadores;

namespace SistemaHostal.Application.Trabajadores;

public class EditarTrabajadorCommandHandler(
    ITrabajadorRepository trabajadorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditarTrabajadorCommand, Result<TrabajadorDto>>
{
    public async Task<Result<TrabajadorDto>> Handle(EditarTrabajadorCommand request, CancellationToken cancellationToken)
    {
        var trabajador = await trabajadorRepository.GetByIdAsync(request.TrabajadorId, cancellationToken);
        if (trabajador is null)
            return Result<TrabajadorDto>.Failure(TrabajadoresError.TrabajadorNoEncontrado, "Trabajador no encontrado.");

        var nombreEnUso = await trabajadorRepository.ExisteNombreAsync(request.Nombre, request.TrabajadorId, cancellationToken);
        if (nombreEnUso)
            return Result<TrabajadorDto>.Failure(TrabajadoresError.NombreTrabajadorNoDisponible, "Ya existe un trabajador con ese nombre.");

        trabajador.Editar(request.Nombre);
        trabajadorRepository.Update(trabajador);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new TrabajadorDto(trabajador.Id, trabajador.Nombre, trabajador.Estado.ToString(), trabajador.FechaCreacion, trabajador.FechaModificacion);
        return Result<TrabajadorDto>.Success(dto);
    }
}