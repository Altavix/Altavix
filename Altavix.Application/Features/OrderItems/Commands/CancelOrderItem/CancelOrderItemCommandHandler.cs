using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.CancelOrderItem;

public class CancelOrderItemCommandHandler : IRequestHandler<CancelOrderItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public CancelOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(CancelOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

        var item = order.Items.FirstOrDefault(i => i.Id == request.OrderItemId);
        if (item == null)
            throw new KeyNotFoundException($"Order Item with ID {request.OrderItemId} was not found in Order {request.OrderId}.");

        item.Cancelled = DateTime.UtcNow;
        item.CancelReason = request.CancelReason;

        // Recalculate total so cancelled item is excluded
        order.CalculateTotal();

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

