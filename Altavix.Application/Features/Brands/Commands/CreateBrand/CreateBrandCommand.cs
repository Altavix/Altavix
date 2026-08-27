using MediatR;

namespace Altavix.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public bool Enabled { get; set; } = true;
}
