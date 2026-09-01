using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Products.Queries.GetProducts;

public class GetAdminProductsListQuery : IRequest<PaginatedList<AdminProductVm>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public Guid[]? BrandIds { get; set; }
    public Guid[]? CategoryIds { get; set; }
    public Dictionary<Guid, string[]>? CharacteristicsFilters { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }

    public GetAdminProductsListQuery(int pageNumber, int pageSize, string? searchTerm = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchTerm = searchTerm;
    }
}
