using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class LineaVentaConfiguration : IEntityTypeConfiguration<LineaVenta>
{
    public void Configure(EntityTypeBuilder<LineaVenta> builder)
    {
        builder.ToTable("LineasVenta");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.NombreProducto).HasMaxLength(150).IsRequired();
        builder.Property(l => l.PrecioUnitario).HasColumnType("numeric(10,2)");

        builder.Ignore(l => l.Subtotal);
    }
}