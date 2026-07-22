namespace SistemaHostal.Application.Catalogo;

public record CategoriaResumenDto(
    int Id,
    string Nombre,
    string Estado,
    int CantidadProductos,
    DateTime FechaCreacion);