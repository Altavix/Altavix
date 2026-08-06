using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;
using Dapper;

namespace Altavix.Application.Features.Products.Queries.GetProductById;

public class GetAdminProductByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetAdminProductByIdQuery, AdminProductVm?>
{
    public GetAdminProductByIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<AdminProductVm?> Handle(GetAdminProductByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id, Title, Description, Price, PriceCoin, CreatedAt, UpdatedAt, UserCreatorId
            FROM tbProducts 
            WHERE Id = @Id;

            SELECT ImageContent FROM tbProductImages WHERE ProductId = @Id;

            SELECT CategoriesId FROM tbCategoryProduct WHERE ProductEntityId = @Id;
        ";

        return await QueryMultipleAsync(sql, async reader =>
        {
            var product = await reader.ReadFirstOrDefaultAsync<AdminProductVm>();
            if (product != null)
            {
                var images = (await reader.ReadAsync<string>()).ToList();
                var categoryIds = (await reader.ReadAsync<Guid>()).ToList();

                product.Images = images;
                product.CategoryIds = categoryIds;
            }
            return product;
        }, new { request.Id });
    }
}
