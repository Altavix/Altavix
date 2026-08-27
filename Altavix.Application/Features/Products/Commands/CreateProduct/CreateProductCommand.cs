using Altavix.Application.Features.Products.DTOs;
using MediatR;

namespace Altavix.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public int PriceCoin { get; set; }
    
    public Guid UserCreatorId { get; set; }
    
    public List<Guid> CategoryIds { get; set; } = new();
    public List<string> Images { get; set; } = new();
    
    public Guid? BrandId { get; set; }
    public bool InStock { get; set; } = true;
    public bool Enabled { get; set; } = true;
    public List<ProductCharacteristicDto> Characteristics { get; set; } = new();
}