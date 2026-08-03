using Altavix.Application.Features.Products.ViewModels;
using MediatR;

namespace Altavix.Application.Features.Products.Queries.GetProductById;

public record GetAdminProductByIdQuery(Guid Id) : IRequest<AdminProductVm?>;
