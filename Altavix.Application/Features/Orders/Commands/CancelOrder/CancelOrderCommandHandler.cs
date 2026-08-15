using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

        if (order.Processing.HasValue || order.Shipped.HasValue || order.Delivered.HasValue)
            throw new InvalidOperationException("Cannot cancel an order that is already in processing or shipped.");

        if (order.Cancelled.HasValue)
            throw new InvalidOperationException("This order is already cancelled.");

        order.Cancelled = DateTime.UtcNow;

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
