using SistemaHostal.Application.Common;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Application.Ventas;

public interface IVentaRepository : IRepository<Venta>
{
    Task<Venta?> ObtenerConLineasYPagosAsync(int ventaId, CancellationToken cancellationToken = default);

    Task<Venta?> ObtenerVentaEnProcesoPorTurnoAsync(int turnoId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Domain.Ventas.Venta>> ObtenerPendientesConTrabajadorAsync(CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Domain.Ventas.Venta>> ObtenerPendientesPorTrabajadorAsync(int trabajadorId, CancellationToken cancellationToken = default);
}