using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Notificaciones;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion>
{
    public void Configure(EntityTypeBuilder<Notificacion> builder)
    {
        builder.ToTable("Notificaciones");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Canal).HasMaxLength(50).IsRequired();
        builder.Property(n => n.Contenido).IsRequired();
        builder.Property(n => n.Estado).HasConversion<string>().HasMaxLength(20);

        builder.Ignore(n => n.DomainEvents);
    }
}