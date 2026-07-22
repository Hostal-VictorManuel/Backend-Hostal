using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class IncidenciaConfiguration : IEntityTypeConfiguration<Incidencia>
{
    public void Configure(EntityTypeBuilder<Incidencia> builder)
    {
        builder.ToTable("Incidencias");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Descripcion).HasMaxLength(500).IsRequired();
    }
}