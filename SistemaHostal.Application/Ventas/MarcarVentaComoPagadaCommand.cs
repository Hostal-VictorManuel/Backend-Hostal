using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Ventas;

public record MarcarVentaComoPagadaCommand(int VentaId, IReadOnlyList<PagoInput> Pagos) : IRequest<Result<VentaDetalleDto>>;