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
        var conditions = new List<string> { "1=1" };
        var parameters = new DynamicParameters();

        parameters.Add("PageNumber", request.PageNumber);
        parameters.Add("PageSize", request.PageSize);

        if (request.MinPrice.HasValue)
        {
            conditions.Add("p.Price >= @MinPrice");
            parameters.Add("MinPrice", request.MinPrice);
        }
        if (request.MaxPrice.HasValue)
        {
            conditions.Add("p.Price <= @MaxPrice");
            parameters.Add("MaxPrice", request.MaxPrice);
        }
        if (request.BrandIds != null && request.BrandIds.Any())
        {
            conditions.Add("p.BrandId IN @BrandIds");
            parameters.Add("BrandIds", request.BrandIds);
        }
        if (request.CategoryIds != null && request.CategoryIds.Any())
        {
            conditions.Add("(SELECT COUNT(1) FROM tbCategoryProduct cp WHERE cp.ProductEntityId = p.Id AND cp.CategoriesId IN @CategoryIds) = @CategoryIdsCount");
            parameters.Add("CategoryIds", request.CategoryIds);
            parameters.Add("CategoryIdsCount", request.CategoryIds.Length);
        }

        if (request.CharacteristicsFilters != null && request.CharacteristicsFilters.Any())
        {
            int charIndex = 0;
            foreach (var kvp in request.CharacteristicsFilters)
            {
                if (kvp.Value == null || !kvp.Value.Any()) continue;
                
                var charIdParam = $"CharId_{charIndex}";
                var charValuesParam = $"CharValues_{charIndex}";
                
                conditions.Add($@"p.Id IN (
                    SELECT pc.ProductId FROM tbProductCharacteristics pc 
                    WHERE pc.CharacteristicId = @{charIdParam} AND pc.Value IN @{charValuesParam}
                )");
                
                parameters.Add(charIdParam, kvp.Key);
                parameters.Add(charValuesParam, kvp.Value);
                charIndex++;
            }
        }

        var whereSql = "WHERE " + string.Join(" AND ", conditions);

        var sql = $@"
            DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

            SELECT COUNT(1) FROM tbProducts p {whereSql};

            SELECT 
                p.Id, p.Title, p.Description, p.Price, p.PriceCoin,
                p.BrandId, p.InStock, p.Enabled, b.Name AS BrandName
            FROM tbProducts p
            LEFT JOIN tbBrands b ON p.BrandId = b.Id
            {whereSql}
            ORDER BY p.CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

        var (totalCount, products) = await QueryMultipleAsync(sql, async reader =>
        {
            var count = await reader.ReadSingleAsync<int>();
            var prods = (await reader.ReadAsync<AdminProductVm>()).ToList();
            return (count, prods);
        }, parameters);

        if (products.Any())
        {
            var productIds = products.Select(p => p.Id).ToArray();
            
            const string relatedSql = @"
                SELECT ProductEntityId, CategoriesId FROM tbCategoryProduct WHERE ProductEntityId IN @ProductIds;
                SELECT pc.ProductId, pc.CharacteristicId, pc.Value, c.Name 
                FROM tbProductCharacteristics pc
                INNER JOIN tbCharacteristics c ON pc.CharacteristicId = c.Id
                WHERE pc.ProductId IN @ProductIds;
            ";

            // Execute Image query completely separately to avoid MARS internal CLR crash
            var images = (await QueryAsync<dynamic>(
                "SELECT ProductId, ImageContent FROM tbProductImages WHERE ProductId IN @ProductIds", 
                new { ProductIds = productIds })).ToList();

            await QueryMultipleAsync(relatedSql, async reader =>
            {
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
