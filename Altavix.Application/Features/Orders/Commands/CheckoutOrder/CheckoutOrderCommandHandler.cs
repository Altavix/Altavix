using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        
        if (order == null)
            throw new KeyNotFoundException($"Cart/Order with ID {request.OrderId} was not found.");

        if (order.Ordered.HasValue)
            throw new InvalidOperationException("This cart has already been checked out as an order.");

        if (order.Items.Count == 0)
            throw new InvalidOperationException("Cannot checkout an empty cart.");

        // Apply checkout info
        order.ClientName = request.ClientName;
        order.ClientMobilePhone = request.ClientMobilePhone;
        order.ClientEmail = request.ClientEmail;
        order.City = request.City;
        order.CityRef = request.CityRef;
        order.Address = request.Address;
        order.Comment = request.Comment;
        order.DeliveryMethodId = request.DeliveryMethodId;
        order.PaymentMethodId = request.PaymentMethodId;
        
        // Mark as Ordered and Processing
        order.Ordered = DateTime.UtcNow;
        order.Processing = DateTime.UtcNow;

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
