namespace Altavix.Domain;

public class ProductImageEntity
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public string ImageContent { get; set; } = string.Empty;
}
