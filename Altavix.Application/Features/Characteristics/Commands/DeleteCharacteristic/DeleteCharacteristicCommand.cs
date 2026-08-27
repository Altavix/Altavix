using MediatR;

namespace Altavix.Application.Features.Characteristics.Commands.DeleteCharacteristic;

public class DeleteCharacteristicCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
