using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record CerrarMesTrabajadoresCommand(int MetodoDePagoId) : IRequest<Result<int>>;