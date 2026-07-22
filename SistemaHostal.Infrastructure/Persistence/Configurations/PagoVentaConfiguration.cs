using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Ventas;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class PagoVentaConfiguration : IEntityTypeConfiguration<PagoVenta>
{
    public void Configure(EntityTypeBuilder<PagoVenta> builder)
    {
        builder.ToTable("PagosVenta");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Monto).HasColumnType("numeric(10,2)");
        builder.Property(p => p.ReferenciaPago).HasMaxLength(100);
    }
}