using SistemaHostal.Application.Auditoria;
using SistemaHostal.Application.Catalogo;

namespace SistemaHostal.Application.Reportes;

public interface IReportesQueries
{
    Task<DashboardRecepcionistaDto> ObtenerDashboardRecepcionistaAsync(CancellationToken cancellationToken = default);

    Task<DashboardAdministradorDto> ObtenerDashboardAdministradorAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductoMasVendidoDto>> ObtenerProductosMasVendidosAsync(
        DateTime? desde, DateTime? hasta, int top, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductoMasVendidoDto>> ObtenerProductosMenorRotacionAsync(
        DateTime? desde, DateTime? hasta, int top, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VentasPorDiaDto>> ObtenerVentasPorPeriodoAsync(
        DateTime desde, DateTime hasta, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VentasPorMetodoPagoDto>> ObtenerVentasPorMetodoPagoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RegistroBitacoraDto>> ObtenerUltimosMovimientosAsync(int cantidad, CancellationToken cancellationToken = default);

    Task<FlujoCajaMensualDto> ObtenerFlujoCajaMensualAsync(int anio, int mes, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IngresoPorTurnoDto>> ObtenerIngresosPorTurnoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VentasPorRecepcionistaDto>> ObtenerVentasPorRecepcionistaAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VentaPagoMixtoDto>> ObtenerVentasConPagoMixtoAsync(
        DateTime? desde, DateTime? hasta, CancellationToken cancellationToken = default);
}