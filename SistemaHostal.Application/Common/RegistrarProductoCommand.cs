using MediatR;
using SistemaHostal.Application.Common;

namespace SistemaHostal.Application.Catalogo;

public record RegistrarProductoCommand(
    string CodigoBarras, string Nombre, 
    int CategoriaId, decimal Precio,
    int StockInicial, int StockMinimo, 
    string? ImagenUrl, int UsuarioId) : IRequest<Result<ProductoResumenDto>>;