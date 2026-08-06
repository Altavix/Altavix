namespace Altavix.Domain;

public class OrderItemEntity
{
    public Guid Id { get; set; }
    
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; } = null!;
    
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitPriceCoin { get; set; }
}
