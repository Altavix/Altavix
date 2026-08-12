using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.CancelOrderItem;

public class CancelOrderItemCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
    public string CancelReason { get; set; } = string.Empty;
}
