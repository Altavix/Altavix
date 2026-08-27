using Altavix.Application.Features.Characteristics.DTOs;
using Altavix.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Features.Characteristics.Queries.GetCharacteristicById;

public class GetCharacteristicByIdQueryHandler : IRequestHandler<GetCharacteristicByIdQuery, CharacteristicDto>
{
    private readonly IAltavixDbContext _context;

    public GetCharacteristicByIdQueryHandler(IAltavixDbContext context)
    {
        _context = context;
    }

    public async Task<CharacteristicDto> Handle(GetCharacteristicByIdQuery request, CancellationToken cancellationToken)
    {
        var characteristic = await _context.Characteristics
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (characteristic == null) return null;

        return new CharacteristicDto
        {
            Id = characteristic.Id,
            Name = characteristic.Name,
            Enabled = characteristic.Enabled
        };
    }
}
