using Microsoft.EntityFrameworkCore;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Domain.Identidad;
using SistemaHostal.Domain.Catalogo;
using SistemaHostal.Domain.Pagos;
using SistemaHostal.Domain.Turnos;
using SistemaHostal.Domain.Ventas;
using SistemaHostal.Domain.Inventario;
using SistemaHostal.Domain.Auditoria;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Infrastructure.Persistence;

public class SistemaHostalDbContext(DbContextOptions<SistemaHostalDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<IntentoAcceso> IntentosAcceso => Set<IntentoAcceso>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<MetodoDePago> MetodosDePago => Set<MetodoDePago>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();
    public DbSet<RegistroBitacora> RegistrosBitacora => Set<RegistroBitacora>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SistemaHostalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}