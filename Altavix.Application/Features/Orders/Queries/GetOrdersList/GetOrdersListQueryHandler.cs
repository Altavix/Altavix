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

        var whereClauses = new List<string>();
        
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
            { "Ordered", "o.Ordered" },
            { "ClientName", "o.ClientName" },
            { "City", "o.City" },
            { "TotalPrice", "o.TotalPrice" },
            { "PaymentMethodTitle", "pm.Title" }
        };

        if (request.Filters != null && request.Filters.Any())
        {
            foreach (var filter in request.Filters)
            {
                if (string.IsNullOrEmpty(filter.Value) || string.IsNullOrEmpty(filter.Key)) continue;

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

        var sortColumn = "o.Created";
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
            {whereClause}
            GROUP BY 
                o.Id, o.Number, o.Created, o.Updated, o.Ordered, 
                o.Paid, o.Processing, o.Shipped, o.Delivered, o.Cancelled,
                o.ClientName, o.City, o.Address,
                pm.Title,
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
