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

        // Update Categories safely
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

        // Update Images safely using standard tracking collection
        var existingImages = entity.Images.ToList();
        
        // Remove images that are no longer present
        var imagesToRemove = existingImages.Where(i => !request.Images.Contains(i.ImageContent)).ToList();
        foreach (var img in imagesToRemove)
        {
            entity.Images.Remove(img);
        }

        // Add new images
        var newImageContents = request.Images.Where(img => !existingImages.Any(e => e.ImageContent == img)).ToList();
        foreach (var imgContent in newImageContents)
        {
            entity.Images.Add(new ProductImageEntity 
            {
                Id = Guid.NewGuid(),
                ProductId = entity.Id,
                ImageContent = imgContent
            });
        }
        
        _productRepository.Update(entity);

        try 
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var failingEntities = string.Join(", ", ex.Entries.Select(e => e.Entity.GetType().Name));
            throw new Exception($"Concurrency exception on entities: {failingEntities}. Details: {ex.Message}");
        }

        return Unit.Value;
    }
}
