using Altavix.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Altavix.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Number)
            .HasColumnOrder(1)
            .HasDefaultValueSql("NEXT VALUE FOR OrderNumbers");
            
        builder.HasIndex(o => o.Number)
            .IsUnique();

        builder.Ignore(o => o.Status);

        builder.Property(o => o.ClientName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(o => o.ClientMobilePhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.ClientEmail)
            .HasMaxLength(150);

        builder.Property(o => o.City)
            .HasMaxLength(150);

        builder.Property(o => o.CityRef)
            .HasMaxLength(50);

        builder.Property(o => o.Address)
            .HasMaxLength(500);

        builder.Property(o => o.Comment)
            .HasMaxLength(1000);

        builder.Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // Foreign keys to lookup tables
        builder.HasOne(o => o.DeliveryMethod)
            .WithMany()
            .HasForeignKey(o => o.DeliveryMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.PaymentMethod)
            .WithMany()
            .HasForeignKey(o => o.PaymentMethodId)
            .OnDelete(DeleteBehavior.Restrict);

        // One to Many for Items
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
