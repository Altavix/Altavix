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

    public AltavixDbContext(DbContextOptions<AltavixDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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