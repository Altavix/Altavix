using Altavix.Application.Enums;
using Altavix.Application.Models;
using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.AddOrderItem;

public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, ApiResponseDto<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderItemRepository _orderItemRepository;

    public AddOrderItemCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository, IUnitOfWork unitOfWork,
        IOrderItemRepository orderItemRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _orderItemRepository = orderItemRepository;
    }

    public async Task<ApiResponseDto<Guid>> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
            if (order == null)
                return new ApiResponseDto<Guid> { Message = $"Order with ID {request.OrderId} was not found.", Type = ResponseMessageType.Error };

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return new ApiResponseDto<Guid> { Message = $"Product with ID {request.ProductId} was not found.", Type = ResponseMessageType.Error };

            var orderItem = new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                UnitPrice = product.Price,
                UnitPriceCoin = product.PriceCoin,
                Created = DateTime.UtcNow
            };

            order.AddItem(orderItem);

            await _orderItemRepository.AddAsync(orderItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ApiResponseDto<Guid>
            {
                Data = orderItem.Id,
                Message = "Товар додано в кошик",
                Type = ResponseMessageType.Success
            };
        }
        catch (Exception ex)
        {
            System.IO.File.WriteAllText("A:\\VisualStudioProject\\Altavix\\AltavixAPI\\AddOrderItemError.txt", ex.ToString());
            return new ApiResponseDto<Guid> { Message = "Виникла помилка під час збереження", Type = ResponseMessageType.Error };
        }
    }
}

