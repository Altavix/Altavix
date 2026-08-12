using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCartCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            // Ordered remains null (this is a cart)
            ClientId = request.ClientId
        };

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
