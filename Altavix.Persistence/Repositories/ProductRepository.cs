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
        return await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Images)
            .Include(p => p.Characteristics)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
