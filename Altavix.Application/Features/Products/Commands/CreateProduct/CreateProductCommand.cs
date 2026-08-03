using MediatR;

namespace Altavix.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public int PriceCoin { get; set; }
    
    // UserCreatorId is extracted from Token in Controller
    public Guid UserCreatorId { get; set; }
    
    public List<Guid> CategoryIds { get; set; } = new();
    public List<string> Images { get; set; } = new();
}