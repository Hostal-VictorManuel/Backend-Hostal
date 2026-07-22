using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Identidad;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.NombreCompleto).HasMaxLength(150).IsRequired();
        builder.Property(u => u.NombreUsuario).HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.NombreUsuario).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();

        builder.Property(u => u.Rol).HasConversion<string>().HasMaxLength(30);
        builder.Property(u => u.Estado).HasConversion<string>().HasMaxLength(20);

        builder.Ignore(u => u.DomainEvents);
    }
}