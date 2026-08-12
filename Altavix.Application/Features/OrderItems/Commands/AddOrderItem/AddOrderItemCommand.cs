using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.AddOrderItem;

public class AddOrderItemCommand : IRequest<Guid>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
