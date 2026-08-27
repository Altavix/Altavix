namespace Altavix.Domain;

public class ProductEntity
{
    public Guid Id {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public int Price {get; set;}
    public int PriceCoin {get; set;}
    public Guid UserCreatorId {get; set;}
    public UserEntity UserCreator {get; set;}
    public List<CategoryEntity> Categories { get; set; } = new();
    public List<ProductImageEntity> Images { get; set; } = new();
    
    public bool InStock { get; set; } = true;
    public bool Enabled { get; set; } = true;
    
    public Guid? BrandId { get; set; }
    public BrandEntity Brand { get; set; }
    
    public List<ProductCharacteristicEntity> Characteristics { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}