using Altavix.Application.Enums;
using Altavix.Application.Models;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.RemoveOrderItem;

public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, ApiResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<ApiResponseDto<bool>> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        if (order == null)
            return new ApiResponseDto<bool> { Message = $"Order with ID {request.OrderId} was not found.", Type = ResponseMessageType.Error };

        if (order.Processing.HasValue || order.Shipped.HasValue || order.Delivered.HasValue || order.Cancelled.HasValue)
            return new ApiResponseDto<bool> { Message = "Замовлення вже в обробці або скасовано. Редагування неможливе.", Type = ResponseMessageType.Error };

        var item = order.Items.FirstOrDefault(i => i.Id == request.OrderItemId);
        if (item == null)
            return new ApiResponseDto<bool> { Message = $"Order Item with ID {request.OrderItemId} was not found.", Type = ResponseMessageType.Error };

        order.RemoveItem(item);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResponseDto<bool>
        {
            Data = true,
            Message = "Товар видалено з кошика",
            Type = ResponseMessageType.Success
        };
    }
}

