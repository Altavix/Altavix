using Altavix.Application.Features.Brands.DTOs;
using Altavix.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Features.Brands.Queries.GetBrandById;

public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto>
{
    private readonly IAltavixDbContext _context;

    public GetBrandByIdQueryHandler(IAltavixDbContext context)
    {
        _context = context;
    }

    public async Task<BrandDto> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (brand == null) return null;

        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name,
            Enabled = brand.Enabled
        };
    }
}
