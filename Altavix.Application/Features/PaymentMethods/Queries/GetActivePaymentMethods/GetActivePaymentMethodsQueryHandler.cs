using Altavix.Application.Features.PaymentMethods.ViewModels;
using Altavix.Application.Interfaces;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.PaymentMethods.Queries.GetActivePaymentMethods;

public class GetActivePaymentMethodsQueryHandler : BaseQueryHandler, IRequestHandler<GetActivePaymentMethodsQuery, IEnumerable<PaymentMethodVm>>
{
    public GetActivePaymentMethodsQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<IEnumerable<PaymentMethodVm>> Handle(GetActivePaymentMethodsQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT Id, Title, Type 
            FROM tbPaymentMethods 
            WHERE IsActive = 1;
        ";

        return await QueryAsync<PaymentMethodVm>(sql);
    }
}
