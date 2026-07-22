using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Turnos;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class TurnoConfiguration : IEntityTypeConfiguration<Turno>
{
    public void Configure(EntityTypeBuilder<Turno> builder)
    {
        builder.ToTable("Turnos");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Estado).HasConversion<string>().HasMaxLength(20);

        builder.HasMany(t => t.Incidencias)
            .WithOne()
            .HasForeignKey(i => i.TurnoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.Incidencias).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(t => t.DomainEvents);
    }
}