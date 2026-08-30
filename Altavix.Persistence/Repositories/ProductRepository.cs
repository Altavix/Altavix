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
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            
        if (product != null)
        {
            // Use ToList() instead of ToListAsync() to avoid Microsoft.Data.SqlClient internal CLR crash
            // when reading massive LOB (nvarchar(max)) columns with async MARS connections.
            product.Images = _context.Set<ProductImageEntity>()
                .Where(i => i.ProductId == id)
                .ToList();
        }
        
        return product;
    }
}
