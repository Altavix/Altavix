namespace Altavix.Domain;

public class ProductCharacteristicEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; }
    
    public Guid CharacteristicId { get; set; }
    public CharacteristicEntity Characteristic { get; set; }
    
    public string Value { get; set; }
}
