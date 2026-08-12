using Altavix.Application.Features.DeliveryMethods.ViewModels;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Queries.GetActiveDeliveryMethods;

public class GetActiveDeliveryMethodsQuery : IRequest<IEnumerable<DeliveryMethodVm>>
{
}
