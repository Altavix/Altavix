using Altavix.Domain;
using Altavix.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        
        var entity = await _productRepository.GetProductWithDetailsAsync(request.Id, cancellationToken);

        if (entity == null)
            throw new Exception($"Product with id {request.Id} not found.");

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Price = request.Price;
        entity.PriceCoin = request.PriceCoin;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.BrandId = request.BrandId;
        entity.InStock = request.InStock;
        entity.Enabled = request.Enabled;

        var existingCategoryIds = entity.Categories.Select(c => c.Id).ToList();
        
        var categoriesToRemove = entity.Categories.Where(c => !request.CategoryIds.Contains(c.Id)).ToList();
        foreach(var c in categoriesToRemove)
        {
            entity.Categories.Remove(c);
        }
        
        var categoriesToAddIds = request.CategoryIds.Where(id => !existingCategoryIds.Contains(id)).ToList();
        if (categoriesToAddIds.Any())
        {
            var newCategories = _categoryRepository.Where(c => categoriesToAddIds.Contains(c.Id)).ToList();
            
            foreach (var category in newCategories)
            {
                entity.Categories.Add(category);
            }
        }

        if (request.Images != null)
        {
            var existingImages = entity.Images.ToList();
            var requestImagesSet = new HashSet<string>(request.Images);
            
            var imagesToRemove = existingImages.Where(i => !requestImagesSet.Contains(i.ImageContent)).ToList();
            foreach (var img in imagesToRemove)
            {
                entity.Images.Remove(img);
            }

            var existingImagesSet = new HashSet<string>(existingImages.Select(e => e.ImageContent));
            var newImageContents = request.Images.Where(img => !existingImagesSet.Contains(img)).ToList();
            foreach (var imgContent in newImageContents)
            {
                entity.Images.Add(new ProductImageEntity 
                {
                    ProductId = entity.Id,
                    ImageContent = imgContent
                });
            }
        }
        
        var existingCharacteristics = entity.Characteristics.ToList();
        
        var requestedCharacteristicIds = request.Characteristics.Select(c => c.CharacteristicId).ToList();
        var charsToRemove = existingCharacteristics.Where(c => !requestedCharacteristicIds.Contains(c.CharacteristicId)).ToList();
        foreach (var ch in charsToRemove)
        {
            entity.Characteristics.Remove(ch);
        }

        foreach (var reqChar in request.Characteristics)
        {
            var existing = entity.Characteristics.FirstOrDefault(c => c.CharacteristicId == reqChar.CharacteristicId);
            if (existing != null)
            {
                existing.Value = reqChar.Value ?? string.Empty;
            }
            else
            {
                entity.Characteristics.Add(new ProductCharacteristicEntity
                {
                    ProductId = entity.Id,
                    CharacteristicId = reqChar.CharacteristicId,
                    Value = reqChar.Value ?? string.Empty
                });
            }
        }
        
        try 
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var details = string.Join(", ", ex.Entries.Select(e => $"{e.Entity.GetType().Name} ({e.State})"));
            throw new Exception($"Concurrency exception on entities: {details}. Details: {ex.Message}");
        }

        return Unit.Value;
    }
}
