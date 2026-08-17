using Altavix.Application.Features.Orders.ViewModels;
using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQuery : IRequest<PagedOrderResultVm>
{
    public Guid? ClientId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    
    public string? SortColumn { get; set; }
    public string? SortDirection { get; set; }
    public List<KeyValue> Filters { get; set; } = new();
}
