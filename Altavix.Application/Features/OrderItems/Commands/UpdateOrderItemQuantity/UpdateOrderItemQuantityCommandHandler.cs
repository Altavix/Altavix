using Altavix.Application.Enums;
using Altavix.Application.Models;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.UpdateOrderItemQuantity;

public class UpdateOrderItemQuantityCommandHandler : IRequestHandler<UpdateOrderItemQuantityCommand, ApiResponseDto<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderItemQuantityCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<ApiResponseDto<bool>> Handle(UpdateOrderItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        if (order == null)
            return new ApiResponseDto<bool> { Message = $"Order with ID {request.OrderId} was not found.", Type = ResponseMessageType.Error };

        var item = order.Items.FirstOrDefault(i => i.Id == request.OrderItemId);
        if (item == null)
            return new ApiResponseDto<bool> { Message = $"Order Item with ID {request.OrderItemId} was not found.", Type = ResponseMessageType.Error };

        item.Quantity = request.NewQuantity;
        
        // Ensure the order's total price is recalculated
        order.CalculateTotal();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResponseDto<bool>
        {
            Data = true,
            Message = "Кількість оновлено",
            Type = ResponseMessageType.Success
        };
    }
}

