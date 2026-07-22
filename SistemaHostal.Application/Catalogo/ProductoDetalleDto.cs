namespace SistemaHostal.Application.Catalogo;

public record ProductoDetalleDto(
    int Id,
    string CodigoBarras,
    string Nombre,
    int CategoriaId,
    string Categoria,
    decimal Precio,
    int StockActual,
    int StockMinimo,
    string? ImagenUrl,
    string Estado,
    DateTime FechaCreacion);