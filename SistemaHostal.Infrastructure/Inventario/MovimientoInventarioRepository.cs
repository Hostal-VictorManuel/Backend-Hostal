using SistemaHostal.Application.Inventario;
using SistemaHostal.Domain.Inventario;
using SistemaHostal.Infrastructure.Persistence;

namespace SistemaHostal.Infrastructure.Inventario;

public class MovimientoInventarioRepository(SistemaHostalDbContext context)
    : Repository<MovimientoInventario>(context), IMovimientoInventarioRepository;