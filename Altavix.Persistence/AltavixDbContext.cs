using Altavix.Application.Interfaces;
using Altavix.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Altavix.Persistence;

public class AltavixDbContext : IdentityDbContext<UserEntity, RoleEntity, Guid>, IAltavixDbContext
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<DeliveryMethodEntity> DeliveryMethods { get; set; }
    public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }
    public DbSet<BrandEntity> Brands { get; set; }
    public DbSet<CharacteristicEntity> Characteristics { get; set; }
    public DbSet<ProductCharacteristicEntity> ProductCharacteristics { get; set; }

    public AltavixDbContext(DbContextOptions<AltavixDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasSequence<long>("OrderNumbers")
            .StartsAt(10000)
            .IncrementsBy(1);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName) && !tableName.StartsWith("tb"))
            {
                var newName = tableName.Replace("AspNet", "");
                entityType.SetTableName("tb" + newName);
            }
        }
    }
}