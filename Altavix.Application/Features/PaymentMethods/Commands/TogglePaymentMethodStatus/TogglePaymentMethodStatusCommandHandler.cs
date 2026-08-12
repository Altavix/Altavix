using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Commands.TogglePaymentMethodStatus;

public class TogglePaymentMethodStatusCommandHandler : IRequestHandler<TogglePaymentMethodStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public TogglePaymentMethodStatusCommandHandler(IPaymentMethodRepository paymentMethodRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<bool> Handle(TogglePaymentMethodStatusCommand request, CancellationToken cancellationToken)
    {
        var method = await _paymentMethodRepository.GetByIdAsync(request.Id);
        if (method == null)
            throw new KeyNotFoundException($"Payment method with ID {request.Id} not found.");

        method.IsActive = !method.IsActive;

        _paymentMethodRepository.Update(method);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return method.IsActive;
    }
}

