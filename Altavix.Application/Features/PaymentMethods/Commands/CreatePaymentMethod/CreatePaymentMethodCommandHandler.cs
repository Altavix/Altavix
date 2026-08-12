using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;

public class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public CreatePaymentMethodCommandHandler(IPaymentMethodRepository paymentMethodRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<Guid> Handle(CreatePaymentMethodCommand request, CancellationToken cancellationToken)
    {
        var paymentMethod = new PaymentMethodEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Type = request.Type,
            IsActive = request.IsActive
        };

        await _paymentMethodRepository.AddAsync(paymentMethod);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return paymentMethod.Id;
    }
}

