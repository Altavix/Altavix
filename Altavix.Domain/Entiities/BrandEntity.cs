namespace Altavix.Domain;

public class BrandEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; } = true;
    
    public List<ProductEntity> Products { get; set; } = new();
}
