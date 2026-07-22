using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Inventario;

public record RegistrarIngresoCommand(int ProductoId, int Cantidad, int UsuarioId) : IRequest<Result<MovimientoInventarioDto>>;