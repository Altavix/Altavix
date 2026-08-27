using Altavix.Application.Features.Brands.ViewModels;
using MediatR;

namespace Altavix.Application.Features.Brands.Queries.GetBrandsList;

public class GetBrandsListQuery : IRequest<BrandsListVm>
{
    public bool IncludeDisabled { get; set; } = true;
}
