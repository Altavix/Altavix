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
        var parameters = new DynamicParameters();
        parameters.Add("Offset", (request.Page - 1) * request.PageSize);
        parameters.Add("PageSize", request.PageSize);

        var whereClauses = new List<string> { "o.Ordered IS NOT NULL" };
        
        if (request.ClientId.HasValue)
        {
            whereClauses.Add("o.ClientId = @ClientId");
            parameters.Add("ClientId", request.ClientId.Value);
        }

        var columnWhitelist = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", "o.Id" },
            { "Number", "o.Number" },
            { "Created", "o.Created" },
            { "Ordered", "COALESCE(o.Ordered, o.Created)" },
            { "ClientName", "o.ClientName" },
            { "City", "o.City" },
            { "TotalPrice", "o.TotalPrice" },
            { "PaymentMethodTitle", "pm.Title" },
            { "DeliveryMethodTitle", "dm.Title" },
            { "Status", "CASE WHEN o.Cancelled IS NOT NULL THEN 5 WHEN o.Delivered IS NOT NULL THEN 4 WHEN o.Shipped IS NOT NULL THEN 3 WHEN o.Processing IS NOT NULL THEN 2 WHEN o.Ordered IS NOT NULL THEN 1 ELSE 0 END" }
        };

        if (request.Filters != null && request.Filters.Any())
        {
            foreach (var filter in request.Filters)
            {
                if (string.IsNullOrEmpty(filter.Value) || string.IsNullOrEmpty(filter.Key)) continue;

                if (filter.Key.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    var statusConditions = new List<string>();
                    if (filter.Value.Contains("Скасовано")) statusConditions.Add("o.Cancelled IS NOT NULL");
                    if (filter.Value.Contains("Доставлено")) statusConditions.Add("o.Delivered IS NOT NULL");
                    if (filter.Value.Contains("Відправлено")) statusConditions.Add("o.Shipped IS NOT NULL");
                    if (filter.Value.Contains("В обробці")) statusConditions.Add("o.Processing IS NOT NULL");
                    if (filter.Value.Contains("Нове")) statusConditions.Add("(o.Ordered IS NOT NULL AND o.Processing IS NULL AND o.Shipped IS NULL AND o.Delivered IS NULL AND o.Cancelled IS NULL)");
                    
                    if (statusConditions.Any())
                    {
                        whereClauses.Add("(" + string.Join(" OR ", statusConditions) + ")");
                    }
                    continue;
                }
                
                if (filter.Key.Equals("DeliveryMethodTitle", StringComparison.OrdinalIgnoreCase))
                {
                    if (filter.Value.Contains(","))
                    {
                        var values = filter.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList();
                        whereClauses.Add("dm.Title IN @Filter_DeliveryMethodTitle");
                        parameters.Add("Filter_DeliveryMethodTitle", values);
                    }
                    else
                    {
                        whereClauses.Add("dm.Title LIKE @Filter_DeliveryMethodTitle");
                        parameters.Add("Filter_DeliveryMethodTitle", $"%{filter.Value}%");
                    }
                    continue;
                }

                if (filter.Key.Equals("MinTotalPrice", StringComparison.OrdinalIgnoreCase) && decimal.TryParse(filter.Value, out var minPrice))
                {
                    whereClauses.Add("o.TotalPrice >= @MinTotalPrice");
                    parameters.Add("MinTotalPrice", minPrice);
                    continue;
                }
                if (filter.Key.Equals("MaxTotalPrice", StringComparison.OrdinalIgnoreCase) && decimal.TryParse(filter.Value, out var maxPrice))
                {
                    whereClauses.Add("o.TotalPrice <= @MaxTotalPrice");
                    parameters.Add("MaxTotalPrice", maxPrice);
                    continue;
                }
                
                if (filter.Key.Equals("MinCreated", StringComparison.OrdinalIgnoreCase) && DateTime.TryParse(filter.Value, out var minCreated))
                {
                    whereClauses.Add("o.Created >= @MinCreated");
                    parameters.Add("MinCreated", minCreated);
                    continue;
                }
                if (filter.Key.Equals("MaxCreated", StringComparison.OrdinalIgnoreCase) && DateTime.TryParse(filter.Value, out var maxCreated))
                {
                    // include the whole day
                    whereClauses.Add("o.Created <= @MaxCreated");
                    parameters.Add("MaxCreated", maxCreated.Date.AddDays(1).AddTicks(-1));
                    continue;
                }

                if (columnWhitelist.TryGetValue(filter.Key, out var sqlColumn))
                {
                    var paramName = "Filter_" + filter.Key;
                    
                    if (filter.Key.Equals("Number", StringComparison.OrdinalIgnoreCase) && long.TryParse(filter.Value, out var numberVal))
                    {
                        whereClauses.Add($"{sqlColumn} = @{paramName}");
                        parameters.Add(paramName, numberVal);
                    }
                    else if (filter.Value.Contains(","))
                    {
                        var values = filter.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList();
                        whereClauses.Add($"{sqlColumn} IN @{paramName}");
                        parameters.Add(paramName, values);
                    }
                    else
                    {
                        whereClauses.Add($"{sqlColumn} LIKE @{paramName}");
                        parameters.Add(paramName, $"%{filter.Value}%");
                    }
                }
            }
        }

        var whereClause = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";

        var sortColumn = "COALESCE(o.Ordered, o.Created)";
        var sortDirection = "DESC";

        if (!string.IsNullOrEmpty(request.SortColumn) && columnWhitelist.TryGetValue(request.SortColumn, out var mappedSortCol))
        {
            sortColumn = mappedSortCol;
        }
        
        if (!string.IsNullOrEmpty(request.SortDirection) && 
            (request.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase) || request.SortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase)))
        {
            sortDirection = request.SortDirection.ToUpper();
        }

        var sql = $@"
            -- Query 1: Total Count
            SELECT COUNT(*) 
            FROM tbOrders o
            LEFT JOIN tbPaymentMethods pm ON o.PaymentMethodId = pm.Id
            LEFT JOIN tbDeliveryMethods dm ON o.DeliveryMethodId = dm.Id
            {whereClause};

            -- Query 2: Paged Results
            SELECT 
                o.Id, o.Number, o.Created, o.Updated, o.Ordered, 
                o.Paid, o.Processing, o.Shipped, o.Delivered, o.Cancelled,
                o.ClientName, o.City, o.Address,
                pm.Title AS PaymentMethodTitle,
                dm.Title AS DeliveryMethodTitle,
                o.TotalPrice, o.TotalPriceCoin,
                COALESCE(SUM(oi.Quantity), 0) AS TotalQuantity
            FROM tbOrders o
            LEFT JOIN tbPaymentMethods pm ON o.PaymentMethodId = pm.Id
            LEFT JOIN tbDeliveryMethods dm ON o.DeliveryMethodId = dm.Id
            LEFT JOIN tbOrderItems oi ON o.Id = oi.OrderId
            {whereClause}
            GROUP BY 
                o.Id, o.Number, o.Created, o.Updated, o.Ordered, 
                o.Paid, o.Processing, o.Shipped, o.Delivered, o.Cancelled,
                o.ClientName, o.City, o.Address,
                pm.Title,
                dm.Title,
                o.TotalPrice, o.TotalPriceCoin
            ORDER BY {sortColumn} {sortDirection}
            OFFSET @Offset ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        ";

        return await QueryMultipleAsync(sql, async reader =>
        {
            var result = new PagedOrderResultVm();
            
            result.TotalCount = await reader.ReadFirstAsync<int>();
            result.Orders = (await reader.ReadAsync<OrderSummaryVm>()).ToList();
            
            return result;
        }, parameters);
    }
}
