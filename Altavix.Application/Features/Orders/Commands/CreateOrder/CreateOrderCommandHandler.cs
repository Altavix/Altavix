using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Items == null || !request.Items.Any())
            throw new ArgumentException("Order must contain at least one item.");

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Ordered = DateTime.UtcNow, // Mark as ordered immediately for now
            Processing = DateTime.UtcNow, // Still mark as processing because it's a new order
            
            ClientId = request.ClientId,
            ClientName = request.ClientName,
            ClientMobilePhone = request.ClientMobilePhone,
            ClientEmail = request.ClientEmail,
            
            City = request.City,
            CityRef = request.CityRef,
            Address = request.Address,
            Comment = request.Comment,
            
            DeliveryMethodId = request.DeliveryMethodId,
            PaymentMethodId = request.PaymentMethodId
        };

        // Fetch actual prices from the database for security
        foreach (var itemDto in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} was not found.");

            var orderItem = new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price,
                UnitPriceCoin = product.PriceCoin,
                Created = DateTime.UtcNow,
                Ordered = DateTime.UtcNow,
                Pending = DateTime.UtcNow
            };

            order.AddItem(orderItem);
        }

        // Add to repository
        await _orderRepository.AddAsync(order);
        
        // Save Changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}

