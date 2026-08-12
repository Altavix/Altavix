using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod;

public class CreateDeliveryMethodCommandHandler : IRequestHandler<CreateDeliveryMethodCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;

    public CreateDeliveryMethodCommandHandler(IDeliveryMethodRepository deliveryMethodRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _deliveryMethodRepository = deliveryMethodRepository;
    }

    public async Task<Guid> Handle(CreateDeliveryMethodCommand request, CancellationToken cancellationToken)
    {
        var deliveryMethod = new DeliveryMethodEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Type = request.Type,
            IsActive = request.IsActive
        };

        await _deliveryMethodRepository.AddAsync(deliveryMethod);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return deliveryMethod.Id;
    }
}

