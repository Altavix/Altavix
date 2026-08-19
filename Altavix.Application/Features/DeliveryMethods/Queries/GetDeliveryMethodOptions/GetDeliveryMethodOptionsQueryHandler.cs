using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Queries.GetDeliveryMethodOptions;

public class GetDeliveryMethodOptionsQueryHandler : BaseQueryHandler, IRequestHandler<GetDeliveryMethodOptionsQuery, List<KeyValue>>
{
    public GetDeliveryMethodOptionsQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<List<KeyValue>> Handle(GetDeliveryMethodOptionsQuery request, CancellationToken cancellationToken)
    {
        // For selecting delivery methods in dropdowns (e.g. admin)
        var sql = "SELECT Title AS [Key], Title AS [Value] FROM tbDeliveryMethods WHERE IsActive = 1 ORDER BY Title";
        var result = await QueryAsync<KeyValue>(sql);
        return result.ToList();
    }
}
