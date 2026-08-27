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

        var categoryIds = request.CategoryIds ?? new List<Guid>();
        var images = request.Images ?? new List<string>();
        var characteristics = request.Characteristics ?? new List<Altavix.Application.Features.Products.DTOs.ProductCharacteristicDto>();

        var entity = new ProductEntity()
        {
            Id = productId,
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            Price = request.Price,
            PriceCoin = request.PriceCoin,
            CreatedAt = DateTime.UtcNow,
            UserCreatorId = request.UserCreatorId,
            BrandId = request.BrandId,
            InStock = request.InStock,
            Enabled = request.Enabled,
            Categories = _categoryRepository.Where(c => categoryIds.Contains(c.Id)).ToList(),
            Images = images.Select(img => new ProductImageEntity 
            { 
                Id = Guid.NewGuid(), 
                ProductId = productId, 
                ImageContent = img 
            }).ToList(),
            Characteristics = characteristics.Select(c => new ProductCharacteristicEntity
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                CharacteristicId = c.CharacteristicId,
                Value = c.Value ?? string.Empty
            }).ToList()
        };
        
        await _productRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}