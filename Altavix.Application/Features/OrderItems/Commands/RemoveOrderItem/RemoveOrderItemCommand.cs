using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.RemoveOrderItem;

public class RemoveOrderItemCommand : IRequest<ApiResponseDto<bool>>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
}
