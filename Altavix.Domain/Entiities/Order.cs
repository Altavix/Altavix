using Altavix.Domain.Enums;

namespace Altavix.Domain;

public class OrderEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public OrderStatus Status { get; set; }

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
        TotalPrice = _items.Sum(i => i.UnitPrice * i.Quantity);
        TotalPriceCoin = _items.Sum(i => i.UnitPriceCoin * i.Quantity);
        // Basic normalization if coin > 99
        TotalPrice += TotalPriceCoin / 100;
        TotalPriceCoin %= 100;
    }
}
