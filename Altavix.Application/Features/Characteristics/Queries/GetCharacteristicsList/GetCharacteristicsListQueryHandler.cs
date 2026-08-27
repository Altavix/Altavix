using Altavix.Application.Features.Characteristics.DTOs;
using Altavix.Application.Features.Characteristics.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Features.Characteristics.Queries.GetCharacteristicsList;

public class GetCharacteristicsListQueryHandler : IRequestHandler<GetCharacteristicsListQuery, CharacteristicsListVm>
{
    private readonly IAltavixDbContext _context;

    public GetCharacteristicsListQueryHandler(IAltavixDbContext context)
    {
        _context = context;
    }

    public async Task<CharacteristicsListVm> Handle(GetCharacteristicsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Characteristics.AsNoTracking();

        if (!request.IncludeDisabled)
        {
            query = query.Where(c => c.Enabled);
        }

        var characteristics = await query
            .OrderBy(c => c.Name)
            .Select(c => new CharacteristicDto
            {
                Id = c.Id,
                Name = c.Name,
                Enabled = c.Enabled
            })
            .ToListAsync(cancellationToken);

        return new CharacteristicsListVm { Characteristics = characteristics };
    }
}
