using Altavix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altavix.Persistence.Configurations;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethodEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryMethodEntity> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)");
    }
}
