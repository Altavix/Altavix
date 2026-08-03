namespace Altavix.Application.Features.Products.ViewModels;

public class AdminProductVm : ProductVm
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UserCreatorId { get; set; }
}
