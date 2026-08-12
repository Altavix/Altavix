using Altavix.Domain.Enums;
using MediatR;

namespace Altavix.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod;

public class CreateDeliveryMethodCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public DeliveryMethodType Type { get; set; }
    public bool IsActive { get; set; } = true;
}
