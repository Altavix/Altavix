using Altavix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altavix.Persistence.Configurations;

public class ProductCharacteristicConfiguration : IEntityTypeConfiguration<ProductCharacteristicEntity>
{
    public void Configure(EntityTypeBuilder<ProductCharacteristicEntity> builder)
    {
        builder.ToTable("tbProductCharacteristics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Characteristics)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Characteristic)
            .WithMany()
            .HasForeignKey(x => x.CharacteristicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
