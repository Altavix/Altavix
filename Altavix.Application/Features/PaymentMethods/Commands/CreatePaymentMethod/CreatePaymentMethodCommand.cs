using Altavix.Domain.Enums;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Commands.CreatePaymentMethod;

public class CreatePaymentMethodCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public PaymentMethodType Type { get; set; }
    public bool IsActive { get; set; } = true;
}
