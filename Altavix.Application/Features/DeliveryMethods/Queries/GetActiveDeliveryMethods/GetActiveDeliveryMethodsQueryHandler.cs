using Altavix.Application.Features.DeliveryMethods.ViewModels;
using Altavix.Application.Interfaces;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Queries.GetActiveDeliveryMethods;

public class GetActiveDeliveryMethodsQueryHandler : BaseQueryHandler, IRequestHandler<GetActiveDeliveryMethodsQuery, IEnumerable<DeliveryMethodVm>>
{
    public GetActiveDeliveryMethodsQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<IEnumerable<DeliveryMethodVm>> Handle(GetActiveDeliveryMethodsQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT Id, Title, Description, Price, Type 
            FROM tbDeliveryMethods 
            WHERE IsActive = 1;
        ";

        return await QueryAsync<DeliveryMethodVm>(sql);
    }
}
