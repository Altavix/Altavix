using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.UpdateOrderItemQuantity;

public class UpdateOrderItemQuantityCommand : IRequest<ApiResponseDto<bool>>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
    public int NewQuantity { get; set; }
}
