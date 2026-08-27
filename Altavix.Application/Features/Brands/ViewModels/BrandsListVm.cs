using Altavix.Application.Features.Brands.DTOs;

namespace Altavix.Application.Features.Brands.ViewModels;

public class BrandsListVm
{
    public List<BrandDto> Brands { get; set; } = new();
}
