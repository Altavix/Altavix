using Altavix.Domain;
using Altavix.Domain.Repositories;

namespace Altavix.Persistence.Repositories;

public class PaymentMethodRepository : BaseRepository<PaymentMethodEntity>, IPaymentMethodRepository
{
    public PaymentMethodRepository(AltavixDbContext context) : base(context)
    {
    }
}
