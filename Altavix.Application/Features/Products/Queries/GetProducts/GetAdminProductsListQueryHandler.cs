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

            SELECT COUNT(1) FROM tbProducts;

            SELECT 
                p.Id, p.Title, p.Description, p.Price, p.PriceCoin, p.CreatedAt, p.UpdatedAt, p.UserCreatorId,
                p.BrandId, p.InStock, p.Enabled, b.Name AS BrandName
            FROM tbProducts p
            LEFT JOIN tbBrands b ON p.BrandId = b.Id
            ORDER BY p.CreatedAt DESC
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
                SELECT ProductId, ImageContent FROM tbProductImages WHERE ProductId IN @ProductIds;
                SELECT ProductEntityId, CategoriesId FROM tbCategoryProduct WHERE ProductEntityId IN @ProductIds;
                SELECT pc.ProductId, pc.CharacteristicId, pc.Value, c.Name 
                FROM tbProductCharacteristics pc
                INNER JOIN tbCharacteristics c ON pc.CharacteristicId = c.Id
                WHERE pc.ProductId IN @ProductIds;
            ";

            await QueryMultipleAsync(relatedSql, async reader =>
            {
                var images = (await reader.ReadAsync<dynamic>()).ToList();
                var categories = (await reader.ReadAsync<dynamic>()).ToList();
                var characteristics = (await reader.ReadAsync<dynamic>()).ToList();

                foreach (var product in products)
                {
                    product.Images = images.Where(i => i.ProductId == product.Id).Select(i => (string)i.ImageContent).ToList();
                    product.CategoryIds = categories.Where(c => c.ProductEntityId == product.Id).Select(c => (Guid)c.CategoriesId).ToList();
                    product.Characteristics = characteristics.Where(c => c.ProductId == product.Id).Select(c => new Altavix.Application.Features.Products.DTOs.ProductCharacteristicDto
                    {
                        CharacteristicId = (Guid)c.CharacteristicId,
                        Name = (string)c.Name,
                        Value = (string)c.Value
                    }).ToList();
                }
                return true;
            }, new { ProductIds = productIds });
        }

        return new PaginatedList<AdminProductVm>(products, totalCount, request.PageNumber, request.PageSize);
    }
}
