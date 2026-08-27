using Altavix.Application.Features.Characteristics.DTOs;
using MediatR;

namespace Altavix.Application.Features.Characteristics.Queries.GetCharacteristicById;

public class GetCharacteristicByIdQuery : IRequest<CharacteristicDto>
{
    public Guid Id { get; set; }
}
