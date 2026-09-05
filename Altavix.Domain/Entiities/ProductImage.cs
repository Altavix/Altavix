namespace Altavix.Domain;

public class ProductImageEntity
{
    public Guid Id { get; set; }
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public string ImagePath { get; set; } = string.Empty;

    public int Position { get; set; }
}
