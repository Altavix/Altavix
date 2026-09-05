using Altavix.Domain;
using Altavix.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Persistence.Repositories;

public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
{
    public ProductRepository(AltavixDbContext context) : base(context)
    {
    }

    public async Task<ProductEntity?> GetProductWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Characteristics)
            .Include(p => p.Images.OrderBy(i => i.Position))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return product;
    }

    public void RemoveImage(ProductImageEntity image)
    {
        _context.Set<ProductImageEntity>().Remove(image);
    }

    public void AddImage(ProductImageEntity image)
    {
        _context.Set<ProductImageEntity>().Add(image);
    }
}
