using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;

namespace Altavix.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,  
        IUserRepository userRepository, 
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = Guid.NewGuid();

        var entity = new ProductEntity()
        {
            Id = productId,
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            PriceCoin = request.PriceCoin,
            CreatedAt = DateTime.UtcNow,
            UserCreator = await _userRepository.GetByIdAsync(request.UserCreatorId),
            UserCreatorId = request.UserCreatorId,
            Categories = _categoryRepository.Where(c => request.CategoryIds.Contains(c.Id)).ToList(),
            Images = request.Images.Select(img => new ProductImageEntity 
            { 
                Id = Guid.NewGuid(), 
                ProductId = productId, 
                ImageContent = img 
            }).ToList()
        };
        
        await _productRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}