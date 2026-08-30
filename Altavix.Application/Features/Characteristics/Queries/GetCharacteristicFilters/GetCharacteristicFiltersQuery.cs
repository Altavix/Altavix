using MediatR;
using Altavix.Application.Features.Characteristics.DTOs;

namespace Altavix.Application.Features.Characteristics.Queries.GetCharacteristicFilters;

public class GetCharacteristicFiltersQuery : IRequest<List<CharacteristicFilterDto>>
{
}

public class CharacteristicFilterDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Values { get; set; } = new();
}
