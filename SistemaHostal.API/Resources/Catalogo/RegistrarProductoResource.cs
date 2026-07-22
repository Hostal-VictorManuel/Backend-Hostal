namespace SistemaHostal.API.Resources.Catalogo;

public record RegistrarProductoResource(
    string CodigoBarras,
    string Nombre,
    int CategoriaId,
    decimal Precio,
    int StockInicial,
    int StockMinimo,
    string? ImagenUrl);