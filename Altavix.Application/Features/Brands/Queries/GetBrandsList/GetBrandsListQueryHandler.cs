using Altavix.Application.Features.Brands.DTOs;
using Altavix.Application.Features.Brands.ViewModels;
using Altavix.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Features.Brands.Queries.GetBrandsList;

public class GetBrandsListQueryHandler : IRequestHandler<GetBrandsListQuery, BrandsListVm>
{
    private readonly IAltavixDbContext _context;

    public GetBrandsListQueryHandler(IAltavixDbContext context)
    {
        _context = context;
    }

    public async Task<BrandsListVm> Handle(GetBrandsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Brands.AsNoTracking();

        if (!request.IncludeDisabled)
        {
            query = query.Where(b => b.Enabled);
        }

        var brands = await query
            .OrderBy(b => b.Name)
            .Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                Enabled = b.Enabled
            })
            .ToListAsync(cancellationToken);

        return new BrandsListVm { Brands = brands };
    }
}
