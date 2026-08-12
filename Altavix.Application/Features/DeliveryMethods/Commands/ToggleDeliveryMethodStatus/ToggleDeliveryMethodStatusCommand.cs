using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Commands.ToggleDeliveryMethodStatus;

public class ToggleDeliveryMethodStatusCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
