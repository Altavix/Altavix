using MediatR;

namespace Altavix.Application.Features.Characteristics.Commands.UpdateCharacteristic;

public class UpdateCharacteristicCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
}
