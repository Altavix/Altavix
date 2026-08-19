using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using Dapper;
using MediatR;

namespace Altavix.Application.Features.Categories.Queries.GetCategoryOptions;

public class GetCategoryOptionsQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoryOptionsQuery, List<KeyValue>>
{
    public GetCategoryOptionsQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<List<KeyValue>> Handle(GetCategoryOptionsQuery request, CancellationToken cancellationToken)
    {
        var sql = "SELECT CAST(Id AS NVARCHAR(50)) AS [Key], Title AS [Value] FROM tbCategories ORDER BY Title";
        var result = await QueryAsync<KeyValue>(sql);
        return result.ToList();
    }
}
