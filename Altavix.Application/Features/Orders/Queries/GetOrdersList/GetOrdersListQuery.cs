using Altavix.Application.Features.Orders.ViewModels;
using MediatR;

namespace Altavix.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQuery : IRequest<PagedOrderResultVm>
{
    public Guid? ClientId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
