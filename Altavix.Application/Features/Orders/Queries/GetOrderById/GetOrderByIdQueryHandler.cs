using Altavix.Application.Features.Orders.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;

namespace Altavix.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetOrderByIdQuery, OrderDetailsVm?>
{
    public GetOrderByIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<OrderDetailsVm?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                o.Id, o.Created, o.Updated, o.Ordered, 
                o.Paid, o.Processing, o.Shipped, o.Delivered, o.Cancelled,
                o.ClientName, o.ClientMobilePhone, o.ClientEmail,
                o.City, o.Address, o.Comment,
                o.DeliveryMethodId, dm.Title AS DeliveryMethodTitle,
                o.PaymentMethodId, pm.Title AS PaymentMethodTitle,
                o.TotalPrice, o.TotalPriceCoin
            FROM tbOrders o
            LEFT JOIN tbDeliveryMethods dm ON o.DeliveryMethodId = dm.Id
            LEFT JOIN tbPaymentMethods pm ON o.PaymentMethodId = pm.Id
            WHERE o.Id = @Id;

            SELECT 
                oi.Id, oi.ProductId, p.Title AS ProductTitle,
                oi.Quantity, oi.UnitPrice, oi.UnitPriceCoin,
                oi.Created, oi.Ordered,
                oi.Pending, oi.ReadyToShip, oi.Shipped, oi.Cancelled, oi.CancelReason
            FROM tbOrderItems oi
            LEFT JOIN tbProducts p ON oi.ProductId = p.Id
            WHERE oi.OrderId = @Id;
        ";

        return await QueryMultipleAsync(sql, async reader =>
        {
            var order = await reader.ReadFirstOrDefaultAsync<OrderDetailsVm>();
            if (order != null)
            {
                var items = (await reader.ReadAsync<OrderItemVm>()).ToList();
                order.Items = items;
            }
            return order;
        }, new { request.Id });
    }
}
