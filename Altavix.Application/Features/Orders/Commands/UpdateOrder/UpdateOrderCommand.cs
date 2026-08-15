using Altavix.Application.Models;
using MediatR;

namespace Altavix.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<ApiResponseDto<bool>>
{
    public Guid OrderId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientMobilePhone { get; set; } = string.Empty;
    public string? ClientEmail { get; set; }
    public string? City { get; set; }
    public string? CityRef { get; set; }
    public string? Address { get; set; }
    public string? Comment { get; set; }
    public Guid? DeliveryMethodId { get; set; }
    public Guid? PaymentMethodId { get; set; }
}
