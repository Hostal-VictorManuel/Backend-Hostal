using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class IntentoAccesoConfiguration : IEntityTypeConfiguration<IntentoAcceso>
{
    public void Configure(EntityTypeBuilder<IntentoAcceso> builder)
    {
        builder.ToTable("IntentosAcceso");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.NombreUsuarioIntentado).HasMaxLength(50).IsRequired();
        builder.Property(i => i.Ip).HasMaxLength(45).IsRequired();
        builder.Property(i => i.Resultado).HasConversion<string>().HasMaxLength(20);

        builder.Ignore(i => i.DomainEvents);
    }
}