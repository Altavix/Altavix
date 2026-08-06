using Altavix.Domain;

namespace Altavix.Domain.Repositories;

public interface IProductRepository : IBaseRepository<ProductEntity>
{
    Task<ProductEntity?> GetProductWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
