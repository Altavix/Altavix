using System.ComponentModel;

namespace Altavix.Domain.Enums;

public enum PaymentMethodType
{
    Custom = 0,
    
    [Description("Накладений платіж (при отриманні)")]
    CashOnDelivery = 1,
    
    [Description("Оплата картою онлайн")]
    OnlineCard = 2,
    
    [Description("Безготівковий розрахунок (без ПДВ)")]
    CashlessWithoutVAT = 3,
    
    [Description("Криптовалюта (USDT)")]
    CryptoUSDT = 4
}
