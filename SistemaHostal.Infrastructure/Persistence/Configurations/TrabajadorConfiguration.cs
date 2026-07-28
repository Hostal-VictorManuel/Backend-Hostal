using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Trabajadores;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class TrabajadorConfiguration : IEntityTypeConfiguration<Trabajador>
{
    public void Configure(EntityTypeBuilder<Trabajador> builder)
    {
        builder.ToTable("Trabajadores");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nombre).HasMaxLength(150).IsRequired();
        builder.HasIndex(t => t.Nombre).IsUnique();

        builder.Property(t => t.Estado).HasConversion<string>().HasMaxLength(20);

        builder.Ignore(t => t.DomainEvents);
    }
}