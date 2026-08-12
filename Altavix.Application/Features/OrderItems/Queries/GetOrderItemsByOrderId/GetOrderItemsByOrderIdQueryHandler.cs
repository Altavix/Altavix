using Altavix.Application.Features.Orders.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByOrderId;

public class GetOrderItemsByOrderIdQueryHandler : BaseQueryHandler, IRequestHandler<GetOrderItemsByOrderIdQuery, IEnumerable<OrderItemVm>>
{
    public GetOrderItemsByOrderIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<IEnumerable<OrderItemVm>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                oi.Id, oi.ProductId, p.Title AS ProductTitle,
                oi.Quantity, oi.UnitPrice, oi.UnitPriceCoin,
                oi.Created, oi.Ordered,
                oi.Pending, oi.ReadyToShip, oi.Shipped, oi.Cancelled, oi.CancelReason
            FROM tbOrderItems oi
            LEFT JOIN tbProducts p ON oi.ProductId = p.Id
            WHERE oi.OrderId = @OrderId;
        ";

        return await QueryAsync<OrderItemVm>(sql, new { request.OrderId });
    }
}
