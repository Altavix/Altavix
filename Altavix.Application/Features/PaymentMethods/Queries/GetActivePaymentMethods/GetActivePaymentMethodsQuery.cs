using Altavix.Application.Features.PaymentMethods.ViewModels;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Queries.GetActivePaymentMethods;

public class GetActivePaymentMethodsQuery : IRequest<IEnumerable<PaymentMethodVm>>
{
}
