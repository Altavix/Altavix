using Altavix.Domain;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Interfaces;

public interface IAltavixDbContext
{
    DbSet<CategoryEntity>  Categories { get; }
    DbSet<ProductEntity>  Products { get; }
    DbSet<UserEntity>   Users { get; }
    DbSet<RoleEntity>   Roles { get; } 

    DbSet<OrderEntity> Orders { get; }
    DbSet<OrderItemEntity> OrderItems { get; }
    DbSet<DeliveryMethodEntity> DeliveryMethods { get; }
    DbSet<PaymentMethodEntity> PaymentMethods { get; }
    DbSet<BrandEntity> Brands { get; }
    DbSet<CharacteristicEntity> Characteristics { get; }
    DbSet<ProductCharacteristicEntity> ProductCharacteristics { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}