using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using MediatR;
using Dapper;

namespace Altavix.Application.Features.Products.Queries.GetProducts;

public class GetAdminProductsListQueryHandler : BaseQueryHandler, IRequestHandler<GetAdminProductsListQuery, PaginatedList<AdminProductVm>>
{
    public GetAdminProductsListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<PaginatedList<AdminProductVm>> Handle(GetAdminProductsListQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

            SELECT COUNT(1) FROM Products;

            SELECT 
                Id, Title, Description, Price, PriceCoin, CreatedAt, UpdatedAt, UserCreatorId
            FROM Products
            ORDER BY CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

        var (totalCount, products) = await QueryMultipleAsync(sql, async reader =>
        {
            var count = await reader.ReadSingleAsync<int>();
            var prods = (await reader.ReadAsync<AdminProductVm>()).ToList();
            return (count, prods);
        }, new { request.PageNumber, request.PageSize });

        if (products.Any())
        {
            var productIds = products.Select(p => p.Id).ToArray();
            
            const string relatedSql = @"
                SELECT ProductId, ImageContent FROM ProductImages WHERE ProductId IN @ProductIds;
                SELECT ProductEntityId, CategoriesId FROM CategoryEntityProductEntity WHERE ProductEntityId IN @ProductIds;
            ";

            await QueryMultipleAsync(relatedSql, async reader =>
            {
                var images = (await reader.ReadAsync<dynamic>()).ToList();
                var categories = (await reader.ReadAsync<dynamic>()).ToList();

                foreach (var product in products)
                {
                    product.Images = images.Where(i => i.ProductId == product.Id).Select(i => (string)i.ImageContent).ToList();
                    product.CategoryIds = categories.Where(c => c.ProductEntityId == product.Id).Select(c => (Guid)c.CategoriesId).ToList();
                }
                return true;
            }, new { ProductIds = productIds });
        }

        return new PaginatedList<AdminProductVm>(products, totalCount, request.PageNumber, request.PageSize);
    }
}
