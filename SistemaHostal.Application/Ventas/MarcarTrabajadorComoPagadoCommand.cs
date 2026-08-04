using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record MarcarTrabajadorComoPagadoCommand(int TrabajadorId, int MetodoDePagoId) : IRequest<Result<int>>;