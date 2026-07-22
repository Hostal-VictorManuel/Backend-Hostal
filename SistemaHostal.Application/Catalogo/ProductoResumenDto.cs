namespace SistemaHostal.Application.Catalogo;

public record ProductoResumenDto(
    int Id,
    string CodigoBarras,
    string Nombre,
    string Categoria,
    decimal Precio,
    int StockActual,
    int StockMinimo,
    string Estado);