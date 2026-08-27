using Altavix.Domain;
using Altavix.Domain.Repositories;

namespace Altavix.Persistence.Repositories;

public class BrandRepository : BaseRepository<BrandEntity>, IBrandRepository
{
    public BrandRepository(AltavixDbContext context) : base(context)
    {
    }
}
