using Altavix.Domain.Enums;
using MediatR;
using System;

namespace Altavix.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public OrderStatus NewStatus { get; set; }
}
