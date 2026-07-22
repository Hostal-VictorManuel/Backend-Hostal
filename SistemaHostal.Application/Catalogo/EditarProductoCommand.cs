using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Catalogo;

public record EditarProductoCommand(
    int ProductoId, 
    string Nombre, 
    int CategoriaId, 
    decimal Precio,
    int StockMinimo, 
    string? ImagenUrl, 
    int UsuarioId) : IRequest<Result<ProductoResumenDto>>;