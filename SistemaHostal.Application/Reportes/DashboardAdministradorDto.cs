using SistemaHostal.Application.Catalogo;

namespace SistemaHostal.Application.Reportes;

public record DashboardAdministradorDto(
    int CantidadVentasHoy,
    decimal TotalVentasHoy,
    int CantidadVentasMes,
    decimal TotalVentasMes,
    IReadOnlyList<ProductoMasVendidoDto> TopProductosMasVendidos,
    IReadOnlyList<ProductoResumenDto> ProductosStockCritico,
    int UsuariosActivos);