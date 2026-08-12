using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Commands.ToggleDeliveryMethodStatus;

public class ToggleDeliveryMethodStatusCommandHandler : IRequestHandler<ToggleDeliveryMethodStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeliveryMethodRepository _deliveryMethodRepository;

    public ToggleDeliveryMethodStatusCommandHandler(IDeliveryMethodRepository deliveryMethodRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _deliveryMethodRepository = deliveryMethodRepository;
    }

    public async Task<bool> Handle(ToggleDeliveryMethodStatusCommand request, CancellationToken cancellationToken)
    {
        var method = await _deliveryMethodRepository.GetByIdAsync(request.Id);
        if (method == null)
            throw new KeyNotFoundException($"Delivery method with ID {request.Id} not found.");

        method.IsActive = !method.IsActive;

        _deliveryMethodRepository.Update(method);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return method.IsActive;
    }
}

