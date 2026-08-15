using Altavix.Application.Features.Orders.ViewModels;
using Altavix.Application.Interfaces;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler : BaseQueryHandler, IRequestHandler<GetOrdersListQuery, PagedOrderResultVm>
{
    public GetOrdersListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<PagedOrderResultVm> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        // Add filtering by ClientId if provided
        var whereClause = request.ClientId.HasValue ? "WHERE ClientId = @ClientId" : "";

        var sql = $@"
            -- Query 1: Total Count
            SELECT COUNT(*) 
            FROM tbOrders
            {whereClause};

            -- Query 2: Paged Results
            SELECT 
                o.Id, o.Number, o.Created, o.Updated, o.Ordered, 
                o.Paid, o.Processing, o.Shipped, o.Delivered, o.Cancelled,
                o.ClientName, o.City, o.Address,
                pm.Title AS PaymentMethodTitle,
                o.TotalPrice, o.TotalPriceCoin,
                COALESCE(SUM(oi.Quantity), 0) AS TotalQuantity
            FROM tbOrders o
            LEFT JOIN tbPaymentMethods pm ON o.PaymentMethodId = pm.Id
            LEFT JOIN tbOrderItems oi ON o.Id = oi.OrderId
            {whereClause.Replace("ClientId = ", "o.ClientId = ")}
            GROUP BY 
                o.Id, o.Number, o.Created, o.Updated, o.Ordered, 
                o.Paid, o.Processing, o.Shipped, o.Delivered, o.Cancelled,
                o.ClientName, o.City, o.Address,
                pm.Title,
                o.TotalPrice, o.TotalPriceCoin
            ORDER BY o.Created DESC
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        ";

        var parameters = new
        {
            ClientId = request.ClientId,
            Offset = (request.Page - 1) * request.PageSize,
            PageSize = request.PageSize
        };

        return await QueryMultipleAsync(sql, async reader =>
        {
            var result = new PagedOrderResultVm();
            
            result.TotalCount = await reader.ReadFirstAsync<int>();
            result.Orders = (await reader.ReadAsync<OrderSummaryVm>()).ToList();
            
            return result;
        }, parameters);
    }
}
