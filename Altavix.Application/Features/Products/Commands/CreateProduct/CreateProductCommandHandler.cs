using Altavix.Application.Interfaces;
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
    private readonly IImageService _imageService;

    public CreateProductCommandHandler(
        IProductRepository productRepository,  
        IUserRepository userRepository, 
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IImageService imageService)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = Guid.NewGuid();

        var categoryIds = request.CategoryIds ?? new List<Guid>();
        var characteristics = request.Characteristics ?? new List<Altavix.Application.Features.Products.DTOs.ProductCharacteristicDto>();

        var productImages = new List<ProductImageEntity>();
        if (request.Images != null && request.Images.Any())
        {
            for (int i = 0; i < request.Images.Count; i++)
            {
                var img = request.Images[i];
                if (string.IsNullOrWhiteSpace(img)) continue;
                var imagePath = await _imageService.SaveImageAsync(img.Trim(), cancellationToken);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    productImages.Add(new ProductImageEntity
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        ImagePath = imagePath,
                        Position = i
                    });
                }
            }
        }

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
            Images = productImages,
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