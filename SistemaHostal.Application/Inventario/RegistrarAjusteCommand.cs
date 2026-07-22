using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Inventario;

public record RegistrarAjusteCommand(int ProductoId, int NuevoStock, string Motivo, int UsuarioId) : IRequest<Result<MovimientoInventarioDto>>;