using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class VentaConfiguration : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> builder)
    {
        builder.ToTable("Ventas");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.NumeroVenta).HasMaxLength(30).IsRequired();
        builder.HasIndex(v => v.NumeroVenta).IsUnique();

        builder.Property(v => v.NumeroHabitacion).HasMaxLength(20);
        builder.Property(v => v.Observaciones).HasMaxLength(500);
        builder.Property(v => v.Estado).HasConversion<string>().HasMaxLength(20);
        builder.Property(v => v.VueltoEfectivo).HasColumnType("numeric(10,2)");

        builder.Ignore(v => v.Total);
        builder.Ignore(v => v.DomainEvents);

        builder.HasMany(v => v.LineasVenta)
            .WithOne()
            .HasForeignKey(l => l.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(v => v.LineasVenta).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(v => v.PagosVenta)
            .WithOne()
            .HasForeignKey(p => p.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(v => v.PagosVenta).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}