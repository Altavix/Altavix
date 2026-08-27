using Altavix.Domain;
using Altavix.Domain.Repositories;

namespace Altavix.Persistence.Repositories;

public class CharacteristicRepository : BaseRepository<CharacteristicEntity>, ICharacteristicRepository
{
    public CharacteristicRepository(AltavixDbContext context) : base(context)
    {
    }
}
