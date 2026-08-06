using Altavix.Application.Features.Categories.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;


namespace Altavix.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoryByIdQuery, CategoryVm?>
{
    public GetCategoryByIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<CategoryVm?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, 
                Title 
            FROM tbCategories 
            WHERE Id = @Id";

        return await QueryFirstOrDefaultAsync<CategoryVm>(sql, new { request.Id });
    }
}

