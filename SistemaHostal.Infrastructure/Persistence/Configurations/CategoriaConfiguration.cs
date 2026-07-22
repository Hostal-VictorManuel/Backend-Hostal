using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
        builder.HasIndex(c => c.Nombre).IsUnique();

        builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20);

        builder.Ignore(c => c.DomainEvents);
    }
}