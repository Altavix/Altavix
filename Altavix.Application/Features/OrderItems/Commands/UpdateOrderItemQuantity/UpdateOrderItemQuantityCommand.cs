using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.UpdateOrderItemQuantity;

public class UpdateOrderItemQuantityCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
    public int NewQuantity { get; set; }
}
