namespace SistemaHostal.API.Resources.Catalogo;

public record EditarProductoResource(
    string Nombre,
    int CategoriaId,
    decimal Precio,
    int StockMinimo,
    string? ImagenUrl);