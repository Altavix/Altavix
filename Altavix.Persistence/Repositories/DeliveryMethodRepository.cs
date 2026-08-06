using Altavix.Domain;
using Altavix.Domain.Repositories;

namespace Altavix.Persistence.Repositories;

public class DeliveryMethodRepository : BaseRepository<DeliveryMethodEntity>, IDeliveryMethodRepository
{
    public DeliveryMethodRepository(AltavixDbContext context) : base(context)
    {
    }
}
