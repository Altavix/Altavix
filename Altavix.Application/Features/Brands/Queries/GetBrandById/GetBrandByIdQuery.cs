using Altavix.Application.Features.Brands.DTOs;
using MediatR;

namespace Altavix.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQuery : IRequest<BrandDto>
{
    public Guid Id { get; set; }
}
