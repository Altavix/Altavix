using MediatR;
using Altavix.Application.Features.Orders.ViewModels; // To reuse OrderItemVm

namespace Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByOrderId;

public class GetOrderItemsByOrderIdQuery : IRequest<IEnumerable<OrderItemVm>>
{
    public Guid OrderId { get; set; }
}
