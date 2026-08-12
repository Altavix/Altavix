namespace Altavix.Application.Features.OrderItems.ViewModels;

public class AdminOrderItemVm
{
    public Guid Id { get; set; }
    
    // Order Info
    public Guid OrderId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? DeliveryMethodTitle { get; set; }

    // Product Info
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    
    // Statuses
    public DateTime Created { get; set; }
    public DateTime? Ordered { get; set; }
    public DateTime? Pending { get; set; }
    public DateTime? ReadyToShip { get; set; }
    public DateTime? Shipped { get; set; }
    public DateTime? Cancelled { get; set; }
}
