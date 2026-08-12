using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.RemoveOrderItem;

public class RemoveOrderItemCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
}
