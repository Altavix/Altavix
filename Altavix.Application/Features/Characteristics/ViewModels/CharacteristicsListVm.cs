using Altavix.Application.Features.Characteristics.DTOs;

namespace Altavix.Application.Features.Characteristics.ViewModels;

public class CharacteristicsListVm
{
    public List<CharacteristicDto> Characteristics { get; set; } = new();
}
