using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Queries.GetDeliveryMethodOptions;

public class GetDeliveryMethodOptionsQuery : IRequest<List<KeyValue>>
{
}
