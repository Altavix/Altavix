using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Queries.GetPaymentMethodOptions;

public class GetPaymentMethodOptionsQueryHandler : BaseQueryHandler, IRequestHandler<GetPaymentMethodOptionsQuery, List<KeyValue>>
{
    public GetPaymentMethodOptionsQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<List<KeyValue>> Handle(GetPaymentMethodOptionsQuery request, CancellationToken cancellationToken)
    {
        // For filtering by PaymentMethodTitle in OrdersMonitor, Key and Value should both be Title
        var sql = "SELECT Title AS [Key], Title AS [Value] FROM tbPaymentMethods WHERE IsActive = 1 ORDER BY Title";
        var result = await QueryAsync<KeyValue>(sql);
        return result.ToList();
    }
}
