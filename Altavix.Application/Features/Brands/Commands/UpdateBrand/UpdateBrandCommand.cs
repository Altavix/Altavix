using MediatR;

namespace Altavix.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
}
