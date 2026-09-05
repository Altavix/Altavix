namespace Altavix.Application.Features.Products.DTOs;

public class ProductCharacteristicRowDto
{
    public Guid ProductId { get; set; }
    public Guid CharacteristicId { get; set; }
    public string Value { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
