using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Queries.GetPaymentMethodOptions;

public class GetPaymentMethodOptionsQuery : IRequest<List<KeyValue>>
{
}
