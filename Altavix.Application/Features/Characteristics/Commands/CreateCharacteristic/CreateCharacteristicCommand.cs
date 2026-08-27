using MediatR;

namespace Altavix.Application.Features.Characteristics.Commands.CreateCharacteristic;

public class CreateCharacteristicCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public bool Enabled { get; set; } = true;
}
