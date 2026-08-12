using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.ShipOrderItem;

public class ShipOrderItemCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
}
