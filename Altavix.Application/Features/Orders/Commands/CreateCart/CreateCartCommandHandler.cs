using Altavix.Application.Enums;
using Altavix.Application.Models;
using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CreateCart;

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ApiResponseDto<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCartCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponseDto<Guid>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        if (request.ClientId.HasValue)
        {
            var existingCart = await _orderRepository.GetActiveCartForUserAsync(request.ClientId.Value, cancellationToken);
            if (existingCart != null)
            {
                return new ApiResponseDto<Guid>
                {
                    Data = existingCart.Id,
                    Message = "Активний кошик знайдено",
                    Type = ResponseMessageType.Success
                };
            }
        }

        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            // Ordered remains null (this is a cart)
            ClientId = request.ClientId
        };

        await _orderRepository.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResponseDto<Guid>
        {
            Data = order.Id,
            Message = "Кошик створено",
            Type = ResponseMessageType.Success
        };
    }
}
