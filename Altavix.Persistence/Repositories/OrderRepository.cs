using Altavix.Domain;
using Altavix.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Persistence.Repositories;

public class OrderRepository : BaseRepository<OrderEntity>, IOrderRepository
{
    public OrderRepository(AltavixDbContext context) : base(context)
    {
    }

    public async Task<OrderEntity?> GetOrderWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Include(o => o.DeliveryMethod)
            .Include(o => o.PaymentMethod)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
