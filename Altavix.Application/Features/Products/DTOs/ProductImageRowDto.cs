namespace Altavix.Application.Features.Products.DTOs;

public class ProductImageRowDto
{
    public Guid ProductId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public int Position { get; set; }
}
