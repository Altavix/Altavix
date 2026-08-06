using Altavix.Domain;

namespace Altavix.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<OrderEntity>
{
    Task<OrderEntity?> GetOrderWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
