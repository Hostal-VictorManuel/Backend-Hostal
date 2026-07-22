using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Pagos;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class MetodoDePagoConfiguration : IEntityTypeConfiguration<MetodoDePago>
{
    public void Configure(EntityTypeBuilder<MetodoDePago> builder)
    {
        builder.ToTable("MetodosDePago");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Nombre).HasMaxLength(50).IsRequired();
        builder.HasIndex(m => m.Nombre).IsUnique();

        builder.Property(m => m.Estado).HasConversion<string>().HasMaxLength(20);

        builder.Ignore(m => m.DomainEvents);
    }
}