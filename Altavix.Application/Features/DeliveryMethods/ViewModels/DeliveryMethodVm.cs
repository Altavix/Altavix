using Altavix.Domain.Enums;

namespace Altavix.Application.Features.DeliveryMethods.ViewModels;

public class DeliveryMethodVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DeliveryMethodType Type { get; set; }
}
