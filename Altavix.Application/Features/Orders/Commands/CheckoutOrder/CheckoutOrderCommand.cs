using MediatR;

namespace Altavix.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    
    // Address & Client Info (required to finalize the order)
    public string ClientName { get; set; } = string.Empty;
    public string ClientMobilePhone { get; set; } = string.Empty;
    public string? ClientEmail { get; set; }
    
    public string? City { get; set; }
    public string? CityRef { get; set; }
    public string? Address { get; set; }
    public string? Comment { get; set; }
    
    public Guid? DeliveryMethodId { get; set; }
    public Guid? PaymentMethodId { get; set; }
    public Guid? ClientId { get; set; }
}
