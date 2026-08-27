using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, bool>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
    {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (brand == null) return false;

        brand.Name = request.Name;
        brand.Enabled = request.Enabled;

        _brandRepository.Update(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
