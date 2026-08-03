namespace Altavix.Application.Features.Products.ViewModels;

public class ProductVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public int PriceCoin { get; set; }
    public List<Guid> CategoryIds { get; set; } = new();
    public List<string> Images { get; set; } = new();
}
