using Altavix.Domain.Enums;

namespace Altavix.Application.Features.Orders.ViewModels;

public class OrderSummaryVm
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
    public string? City { get; set; }
    public string? Address { get; set; }
    
    public string? PaymentMethodTitle { get; set; }
    
    public decimal TotalPrice { get; set; }
    public int TotalPriceCoin { get; set; }
    public int TotalQuantity { get; set; }
}

public class PagedOrderResultVm
{
    public int TotalCount { get; set; }
    public List<OrderSummaryVm> Orders { get; set; } = new();
}
