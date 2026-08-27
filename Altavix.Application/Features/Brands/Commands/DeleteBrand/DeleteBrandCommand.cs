using MediatR;

namespace Altavix.Application.Features.Brands.Commands.DeleteBrand;

public class DeleteBrandCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
