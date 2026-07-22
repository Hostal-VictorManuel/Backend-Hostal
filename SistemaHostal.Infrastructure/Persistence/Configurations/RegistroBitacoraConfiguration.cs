using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Auditoria;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class RegistroBitacoraConfiguration : IEntityTypeConfiguration<RegistroBitacora>
{
    public void Configure(EntityTypeBuilder<RegistroBitacora> builder)
    {
        builder.ToTable("RegistrosBitacora");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.NombreUsuario).HasMaxLength(150);
        builder.Property(r => r.Modulo).HasConversion<string>().HasMaxLength(20);
        builder.Property(r => r.TipoOperacion).HasConversion<string>().HasMaxLength(30);
        builder.Property(r => r.Detalle).HasMaxLength(1000).IsRequired();

        builder.Ignore(r => r.DomainEvents);
    }
}