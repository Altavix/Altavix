using Altavix.Domain.Enums;

namespace Altavix.Application.Features.PaymentMethods.ViewModels;

public class PaymentMethodVm
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public PaymentMethodType Type { get; set; }
}
