using Altavix.Application.Features.OrderItems.ViewModels;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByStatus;

public class GetOrderItemsByStatusQuery : IRequest<IEnumerable<AdminOrderItemVm>>
{
    // Define simple flags to retrieve what the warehouse worker needs.
    // For example, if they need items that are Pending but NOT ReadyToShip yet:
    public bool OnlyPending { get; set; }
    public bool OnlyReadyToShip { get; set; }
    public bool ExcludeCancelled { get; set; } = true;
}
