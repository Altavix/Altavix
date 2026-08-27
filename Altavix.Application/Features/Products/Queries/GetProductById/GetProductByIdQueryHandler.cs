using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;


namespace Altavix.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : BaseQueryHandler, IRequestHandler<GetProductByIdQuery, ProductVm?>
{
    public GetProductByIdQueryHandler(IDbConnectionFactory connectionProvider) : base(connectionProvider)
    {
    }

    public async Task<ProductVm?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                p.Id, p.Title, p.Description, p.Price, p.PriceCoin,
                p.BrandId, p.InStock, p.Enabled, b.Name AS BrandName
            FROM tbProducts p
            LEFT JOIN tbBrands b ON p.BrandId = b.Id
            WHERE p.Id = @Id;

            SELECT ImageContent FROM tbProductImages WHERE ProductId = @Id;

            SELECT CategoriesId FROM tbCategoryProduct WHERE ProductEntityId = @Id;

            SELECT pc.CharacteristicId, pc.Value, c.Name 
            FROM tbProductCharacteristics pc
            INNER JOIN tbCharacteristics c ON pc.CharacteristicId = c.Id
            WHERE pc.ProductId = @Id;
        ";

        return await QueryMultipleAsync(sql, async reader =>
        {
            var product = await reader.ReadFirstOrDefaultAsync<ProductVm>();
            if (product != null)
            {
                var images = (await reader.ReadAsync<string>()).ToList();
                var categoryIds = (await reader.ReadAsync<Guid>()).ToList();
                var characteristics = (await reader.ReadAsync<dynamic>()).ToList();

                product.Images = images;
                product.CategoryIds = categoryIds;
                product.Characteristics = characteristics.Select(c => new Altavix.Application.Features.Products.DTOs.ProductCharacteristicDto
                {
                    CharacteristicId = (Guid)c.CharacteristicId,
                    Name = (string)c.Name,
                    Value = (string)c.Value
                }).ToList();
            }
            return product;
        }, new { request.Id });
    }
}

