using Altavix.Application.Enums;
using Altavix.Application.Models;
using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponseDto<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponseDto<bool>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        
        if (order == null)
            return new ApiResponseDto<bool> { Message = $"Order with ID {request.OrderId} was not found.", Type = ResponseMessageType.Error };

        // Users can edit up to "Ordered" (status 1). Admins can edit up to "Processing" (status 2).
        if ((!request.IsAdmin && order.Processing.HasValue) || order.Shipped.HasValue || order.Delivered.HasValue || order.Cancelled.HasValue)
            return new ApiResponseDto<bool> { Message = "Замовлення вже в обробці або скасовано. Редагування неможливе.", Type = ResponseMessageType.Error };

        order.ClientName = request.ClientName;
        order.ClientMobilePhone = request.ClientMobilePhone;
        order.ClientEmail = request.ClientEmail;
        order.City = request.City;
        order.CityRef = request.CityRef;
        order.Address = request.Address;
        order.Comment = request.Comment;
        order.DeliveryMethodId = request.DeliveryMethodId;
        order.PaymentMethodId = request.PaymentMethodId;

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResponseDto<bool>
        {
            Data = true,
            Message = "Дані замовлення успішно оновлено",
            Type = ResponseMessageType.Success
        };
    }
}
