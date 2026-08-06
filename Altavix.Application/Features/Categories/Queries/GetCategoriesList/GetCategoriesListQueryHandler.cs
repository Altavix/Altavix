using Altavix.Application.Features.Categories.DTOs;
using Altavix.Application.Features.Categories.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;

namespace Altavix.Application.Features.Categories.Queries.GetCategoriesList;

public class GetCategoriesListQueryHandler : BaseQueryHandler, IRequestHandler<GetCategoriesListQuery, CategoriesListVm>
{
    public GetCategoriesListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<CategoriesListVm> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, 
                Title 
            FROM tbCategories";

        var categories = await QueryAsync<CategoryDto>(sql);
        
        return new CategoriesListVm 
        { 
            Categories = categories.ToList() 
        };
    }
}

