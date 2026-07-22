using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaHostal.Domain.Catalogo;

namespace SistemaHostal.Infrastructure.Persistence.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.CodigoBarras).HasMaxLength(50).IsRequired();
        builder.HasIndex(p => p.CodigoBarras).IsUnique();

        builder.Property(p => p.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Precio).HasColumnType("numeric(10,2)");
        builder.Property(p => p.ImagenUrl).HasMaxLength(500);

        builder.Property(p => p.Estado).HasConversion<string>().HasMaxLength(20);

        builder.HasOne<Categoria>()
            .WithMany()
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(p => p.DomainEvents);
    }
}