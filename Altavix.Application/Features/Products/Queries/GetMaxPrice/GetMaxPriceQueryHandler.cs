using Altavix.Application.Interfaces;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.Products.Queries.GetMaxPrice;

public class GetMaxPriceQueryHandler : BaseQueryHandler, IRequestHandler<GetMaxPriceQuery, decimal>
{
    public GetMaxPriceQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider) { }

    public async Task<decimal> Handle(GetMaxPriceQuery request, CancellationToken cancellationToken)
    {
        const string sql = "SELECT ISNULL(MAX(Price), 0) FROM tbProducts WHERE Enabled = 1;";
        return await QueryFirstOrDefaultAsync<decimal>(sql);
    }
}
