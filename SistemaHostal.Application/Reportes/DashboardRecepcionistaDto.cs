using SistemaHostal.Application.Catalogo;

namespace SistemaHostal.Application.Reportes;

public record DashboardRecepcionistaDto(
    int CantidadVentasHoy,
    decimal TotalVentasHoy,
    int ProductosVendidosHoy,
    IReadOnlyList<ProductoResumenDto> ProductosStockCritico);