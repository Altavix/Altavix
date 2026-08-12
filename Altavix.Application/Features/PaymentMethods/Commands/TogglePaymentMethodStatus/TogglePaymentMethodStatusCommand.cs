using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Commands.TogglePaymentMethodStatus;

public class TogglePaymentMethodStatusCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
