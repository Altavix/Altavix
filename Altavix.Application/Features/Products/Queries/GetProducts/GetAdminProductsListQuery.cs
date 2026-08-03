using Altavix.Application.Features.Products.ViewModels;
using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Products.Queries.GetProducts;

public record GetAdminProductsListQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<AdminProductVm>>;
