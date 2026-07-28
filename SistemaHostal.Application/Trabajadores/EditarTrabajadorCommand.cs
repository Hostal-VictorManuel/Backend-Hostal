using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Trabajadores;

public record EditarTrabajadorCommand(int TrabajadorId, string Nombre) : IRequest<Result<TrabajadorDto>>;