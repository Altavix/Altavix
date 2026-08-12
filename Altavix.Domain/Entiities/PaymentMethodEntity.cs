using Altavix.Domain.Enums;

namespace Altavix.Domain;

public class PaymentMethodEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public PaymentMethodType Type { get; set; } = PaymentMethodType.Custom;
    public bool IsActive { get; set; } = true;
}
