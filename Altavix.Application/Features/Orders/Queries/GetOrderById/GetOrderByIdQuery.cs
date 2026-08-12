using Altavix.Application.Features.Orders.ViewModels;
using MediatR;

namespace Altavix.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQuery : IRequest<OrderDetailsVm?>
{
    public Guid Id { get; set; }
}
