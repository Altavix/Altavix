using Altavix.Application.Features.Products.ViewModels;
using MediatR;

using Altavix.Application.Models;

namespace Altavix.Application.Features.Products.Queries.GetProducts;

public record GetProductsListQuery(
    int PageNumber = 1, 
    int PageSize = 10,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    Guid[]? BrandIds = null,
    Guid[]? CategoryIds = null,
    Dictionary<Guid, string[]>? CharacteristicsFilters = null
) : IRequest<PaginatedList<ProductVm>>;
