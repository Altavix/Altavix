using Altavix.Application.Features.Characteristics.ViewModels;
using MediatR;

namespace Altavix.Application.Features.Characteristics.Queries.GetCharacteristicsList;

public class GetCharacteristicsListQuery : IRequest<CharacteristicsListVm>
{
    public bool IncludeDisabled { get; set; } = true;
}
