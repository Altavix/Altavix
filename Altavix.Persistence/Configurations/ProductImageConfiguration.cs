using Altavix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altavix.Persistence.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageEntity>
{
    public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder.ToTable("tbProductImages");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ImageContent)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
    }
}
