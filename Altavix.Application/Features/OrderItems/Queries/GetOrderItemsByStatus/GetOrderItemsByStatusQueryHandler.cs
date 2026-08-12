using Altavix.Application.Features.OrderItems.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;

namespace Altavix.Application.Features.OrderItems.Queries.GetOrderItemsByStatus;

public class GetOrderItemsByStatusQueryHandler : BaseQueryHandler, IRequestHandler<GetOrderItemsByStatusQuery, IEnumerable<AdminOrderItemVm>>
{
    public GetOrderItemsByStatusQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<IEnumerable<AdminOrderItemVm>> Handle(GetOrderItemsByStatusQuery request, CancellationToken cancellationToken)
    {
        string whereClause = "WHERE 1=1";

        if (request.ExcludeCancelled)
            whereClause += " AND oi.Cancelled IS NULL";

        if (request.OnlyPending)
            whereClause += " AND oi.Pending IS NOT NULL AND oi.ReadyToShip IS NULL AND oi.Shipped IS NULL";

        if (request.OnlyReadyToShip)
            whereClause += " AND oi.ReadyToShip IS NOT NULL AND oi.Shipped IS NULL";

        string sql = $@"
            SELECT 
                oi.Id, 
                oi.OrderId, o.ClientName, o.City, o.Address, dm.Title AS DeliveryMethodTitle,
                oi.ProductId, p.Title AS ProductTitle, oi.Quantity,
                oi.Created, oi.Ordered, oi.Pending, oi.ReadyToShip, oi.Shipped, oi.Cancelled
            FROM tbOrderItems oi
            INNER JOIN tbOrders o ON oi.OrderId = o.Id
            LEFT JOIN tbDeliveryMethods dm ON o.DeliveryMethodId = dm.Id
            INNER JOIN tbProducts p ON oi.ProductId = p.Id
            {whereClause}
            ORDER BY oi.Created DESC;
        ";

        return await QueryAsync<AdminOrderItemVm>(sql);
    }
}
