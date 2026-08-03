using Altavix.Application.Interfaces;
using Altavix.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IAltavixDbContext _context;

    public UpdateProductCommandHandler(IAltavixDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity == null)
            throw new Exception($"Product with id {request.Id} not found."); // We should use a proper NotFoundException later

        entity.Title = request.Title;
        entity.Description = request.Description;
        entity.Price = request.Price;
        entity.PriceCoin = request.PriceCoin;
        entity.UpdatedAt = DateTime.UtcNow;

        // Update Categories
        entity.Categories.Clear();
        var newCategories = await _context.Categories
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
        
        foreach (var category in newCategories)
        {
            entity.Categories.Add(category);
        }

        // Update Images
        entity.Images.Clear();
        foreach (var img in request.Images)
        {
            entity.Images.Add(new ProductImageEntity 
            {
                Id = Guid.NewGuid(),
                ProductId = entity.Id,
                ImageContent = img
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
