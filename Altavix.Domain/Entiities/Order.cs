using Altavix.Domain.Enums;

namespace Altavix.Domain;

public class OrderEntity
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    // State Tracking
    public DateTime? Ordered { get; set; }
    public DateTime? Paid { get; set; }
    public DateTime? Processing { get; set; }
    public DateTime? Shipped { get; set; }
    public DateTime? Delivered { get; set; }
    public DateTime? Cancelled { get; set; }

    // Computed Status
    public OrderStatus Status 
    {
        get 
        {
            if (Cancelled.HasValue) return OrderStatus.Cancelled;
            if (Delivered.HasValue) return OrderStatus.Delivered;
            if (Shipped.HasValue) return OrderStatus.Shipped;
            if (Paid.HasValue) return OrderStatus.Paid;
            if (Processing.HasValue) return OrderStatus.Processing;
            if (Ordered.HasValue) return OrderStatus.Ordered;
            return OrderStatus.New;
        }
    }

    // Delivery & Payment
    public Guid? DeliveryMethodId { get; set; }
    public DeliveryMethodEntity? DeliveryMethod { get; set; }

    public Guid? PaymentMethodId { get; set; }
    public PaymentMethodEntity? PaymentMethod { get; set; }

    // Client Info
    public Guid? ClientId { get; set; }
    public UserEntity? Client { get; set; }

    public string ClientName { get; set; } = string.Empty;
    public string ClientMobilePhone { get; set; } = string.Empty;
    public string? ClientEmail { get; set; }
    
    // Address Info
    public string? City { get; set; }
    public string? CityRef { get; set; }
    public string? Address { get; set; }
    
    public string? Comment { get; set; }

    // Financial
    public decimal TotalPrice { get; set; }
    public int TotalPriceCoin { get; set; }

    // Navigation
    private readonly List<OrderItemEntity> _items = new();
    public IReadOnlyCollection<OrderItemEntity> Items => _items.AsReadOnly();

    public void AddItem(OrderItemEntity item)
    {
        _items.Add(item);
        CalculateTotal();
    }

    public void RemoveItem(OrderItemEntity item)
    {
        _items.Remove(item);
        CalculateTotal();
    }

    public void CalculateTotal()
    {
        var activeItems = _items.Where(i => !i.Cancelled.HasValue).ToList();
        TotalPrice = activeItems.Sum(i => i.UnitPrice * i.Quantity);
        TotalPriceCoin = activeItems.Sum(i => i.UnitPriceCoin * i.Quantity);
        // Basic normalization if coin > 99
        TotalPrice += TotalPriceCoin / 100;
        TotalPriceCoin %= 100;
    }
}
