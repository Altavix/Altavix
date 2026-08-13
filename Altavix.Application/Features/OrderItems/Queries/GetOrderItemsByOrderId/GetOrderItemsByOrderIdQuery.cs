using MediatR;
using Altavix.Application.Features.Orders.ViewModels; // To reuse OrderItemVm
using Altavix.Application.Models;

namespace Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByOrderId;

public class GetOrderItemsByOrderIdQuery : IRequest<ApiResponseDto<IEnumerable<OrderItemVm>>>
{
    public Guid OrderId { get; set; }
}
