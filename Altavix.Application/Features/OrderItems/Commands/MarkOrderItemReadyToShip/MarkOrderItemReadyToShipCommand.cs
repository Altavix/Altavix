using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.MarkOrderItemReadyToShip;

public class MarkOrderItemReadyToShipCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
}
