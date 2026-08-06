using Altavix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altavix.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
