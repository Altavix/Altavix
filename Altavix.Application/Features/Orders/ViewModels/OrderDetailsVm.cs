using Altavix.Domain.Enums;

namespace Altavix.Application.Features.Orders.ViewModels;

public class OrderItemVm
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitPriceCoin { get; set; }

    public DateTime Created { get; set; }
    public DateTime? Ordered { get; set; }
    public DateTime? Pending { get; set; }
    public DateTime? ReadyToShip { get; set; }
    public DateTime? Shipped { get; set; }
    public DateTime? Cancelled { get; set; }
    public string? CancelReason { get; set; }
}

public class OrderDetailsVm
{
    public Guid Id { get; set; }
    public long Number { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public DateTime? Ordered { get; set; }
    public DateTime? Paid { get; set; }
    public DateTime? Processing { get; set; }
    public DateTime? Shipped { get; set; }
    public DateTime? Delivered { get; set; }
    public DateTime? Cancelled { get; set; }

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
    
    public string ClientName { get; set; } = string.Empty;
    public string ClientMobilePhone { get; set; } = string.Empty;
    public string? ClientEmail { get; set; }
    
    public string? City { get; set; }
    public string? Address { get; set; }
    public string? Comment { get; set; }
    
    public Guid? DeliveryMethodId { get; set; }
    public string? DeliveryMethodTitle { get; set; }
    
    public Guid? PaymentMethodId { get; set; }
    public string? PaymentMethodTitle { get; set; }

    public decimal TotalPrice { get; set; }
    public int TotalPriceCoin { get; set; }

    public List<OrderItemVm> Items { get; set; } = new();
}
