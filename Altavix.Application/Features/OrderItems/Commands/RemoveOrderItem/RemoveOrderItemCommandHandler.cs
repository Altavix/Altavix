using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Commands.RemoveOrderItem;

public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(request.OrderId, cancellationToken);
        if (order == null)
            throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

        var item = order.Items.FirstOrDefault(i => i.Id == request.OrderItemId);
        if (item == null)
            throw new KeyNotFoundException($"Order Item with ID {request.OrderItemId} was not found in Order {request.OrderId}.");

        order.RemoveItem(item);

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}

