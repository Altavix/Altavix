using Altavix.Application.Features.Products.DTOs;
using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Interfaces;
using Altavix.Application.Models;
using MediatR;
using Dapper;

namespace Altavix.Application.Features.Products.Queries.GetProducts;

public class GetProductsListQueryHandler : BaseQueryHandler, IRequestHandler<GetProductsListQuery, PaginatedList<ProductVm>>
{
    public GetProductsListQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<PaginatedList<ProductVm>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        var conditions = new List<string> { "p.Enabled = 1" };
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

        var scoreColumn = "";
        var orderBy = "ORDER BY p.CreatedAt DESC";

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = $"%{request.SearchTerm}%";
            parameters.Add("SearchTerm", searchTerm);
            
            conditions.Add(@"(
                p.Title LIKE @SearchTerm OR 
                p.Description LIKE @SearchTerm OR
                b.Name LIKE @SearchTerm OR
                EXISTS (SELECT 1 FROM tbCategoryProduct cp JOIN tbCategories c ON cp.CategoriesId = c.Id WHERE cp.ProductEntityId = p.Id AND c.Title LIKE @SearchTerm) OR
                EXISTS (SELECT 1 FROM tbProductCharacteristics pc JOIN tbCharacteristics ch ON pc.CharacteristicId = ch.Id WHERE pc.ProductId = p.Id AND (ch.Name LIKE @SearchTerm OR pc.Value LIKE @SearchTerm))
            )");

            scoreColumn = @",
            (
                (CASE WHEN p.Title LIKE @SearchTerm THEN 10000 ELSE 0 END) +
                (CASE WHEN p.Description LIKE @SearchTerm THEN 1000 ELSE 0 END) +
                (CASE WHEN b.Name LIKE @SearchTerm THEN 100 ELSE 0 END) +
                (CASE WHEN EXISTS (SELECT 1 FROM tbCategoryProduct cp JOIN tbCategories c ON cp.CategoriesId = c.Id WHERE cp.ProductEntityId = p.Id AND c.Title LIKE @SearchTerm) THEN 10 ELSE 0 END) +
                (CASE WHEN EXISTS (SELECT 1 FROM tbProductCharacteristics pc JOIN tbCharacteristics ch ON pc.CharacteristicId = ch.Id WHERE pc.ProductId = p.Id AND (ch.Name LIKE @SearchTerm OR pc.Value LIKE @SearchTerm)) THEN 1 ELSE 0 END)
            ) AS SearchScore";
            
            orderBy = "ORDER BY SearchScore DESC, p.CreatedAt DESC";
        }

        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "price_asc":
                    orderBy = "ORDER BY p.Price ASC";
                    break;
                case "price_desc":
                    orderBy = "ORDER BY p.Price DESC";
                    break;
                case "oldest":
                    orderBy = "ORDER BY p.CreatedAt ASC";
                    break;
                case "newest":
                default:
                    orderBy = "ORDER BY p.CreatedAt DESC";
                    break;
            }
        }

        var whereSql = "WHERE " + string.Join(" AND ", conditions);

        var sql = $@"
            DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

            SELECT COUNT(1) FROM tbProducts p LEFT JOIN tbBrands b ON p.BrandId = b.Id {whereSql};

            SELECT 
                p.Id, p.Title, p.Description, p.Price, p.PriceCoin,
                p.BrandId, p.InStock, p.Enabled, b.Name AS BrandName
                {scoreColumn}
            FROM tbProducts p
            LEFT JOIN tbBrands b ON p.BrandId = b.Id
            {whereSql}
            {orderBy}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

        var (totalCount, products) = await QueryMultipleAsync(sql, async reader =>
        {
            var count = await reader.ReadSingleAsync<int>();
            var prods = (await reader.ReadAsync<ProductVm>()).ToList();
            return (count, prods);
        }, parameters, commandTimeout: 120);

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
            
            var images = (await QueryAsync<ProductImageRowDto>(
                "SELECT ProductId, ImagePath, Position FROM tbProductImages WHERE ProductId IN @ProductIds ORDER BY Position ASC", 
                new { ProductIds = productIds }, commandTimeout: 120)).ToList();

            await QueryMultipleAsync(relatedSql, async reader =>
            {
                var categories = (await reader.ReadAsync<CategoryProductRowDto>()).ToList();
                var characteristics = (await reader.ReadAsync<ProductCharacteristicRowDto>()).ToList();

                var imagesDict = images.GroupBy(x => x.ProductId).ToDictionary(g => g.Key, g => g.OrderBy(x => x.Position).Select(x => x.ImagePath).ToList());
                var categoriesDict = categories.GroupBy(x => x.ProductEntityId).ToDictionary(g => g.Key, g => g.Select(x => x.CategoriesId).ToList());
                var characteristicsDict = characteristics.GroupBy(x => x.ProductId).ToDictionary(g => g.Key, g => g.Select(x => new ProductCharacteristicDto
                {
                    CharacteristicId = x.CharacteristicId,
                    Name = x.Name,
                    Value = x.Value
                }).ToList());

                foreach (var product in products)
                {
                    product.Images = imagesDict.TryGetValue(product.Id, out var imgs) ? imgs : new List<string>();
                    product.CategoryIds = categoriesDict.TryGetValue(product.Id, out var cats) ? cats : new List<Guid>();
                    product.Characteristics = characteristicsDict.TryGetValue(product.Id, out var chars) ? chars : new List<ProductCharacteristicDto>();
                }
                return true;
            }, new { ProductIds = productIds });
        }

        return new PaginatedList<ProductVm>(products, totalCount, request.PageNumber, request.PageSize);
    }
}
