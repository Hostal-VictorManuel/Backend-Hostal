using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Trabajadores;

public record ActivarDesactivarTrabajadorCommand(int TrabajadorId, bool Activar) : IRequest<Result>;