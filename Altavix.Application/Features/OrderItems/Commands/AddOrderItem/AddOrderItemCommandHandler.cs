using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.AddOrderItem;

public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public AddOrderItemCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.ProductId} was not found.");

        var orderItem = new OrderItemEntity
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            ProductId = product.Id,
            Quantity = request.Quantity,
            UnitPrice = product.Price,
            UnitPriceCoin = product.PriceCoin,
            Created = DateTime.UtcNow,
            Ordered = DateTime.UtcNow, // It goes straight to ordered since order is already ordered
            Pending = DateTime.UtcNow
        };

        order.AddItem(orderItem);

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return orderItem.Id;
    }
}

