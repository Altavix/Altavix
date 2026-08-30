using MediatR;

namespace Altavix.Application.Features.Products.Queries.GetMaxPrice;

public record GetMaxPriceQuery : IRequest<decimal>;
