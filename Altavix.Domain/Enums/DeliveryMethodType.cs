using System.ComponentModel;

namespace Altavix.Domain.Enums;

public enum DeliveryMethodType
{
    Custom = 0,
    
    [Description("Самовивіз")]
    Pickup = 1,
    
    [Description("Нова Пошта (відділення)")]
    NovaPoshta = 2,
    
    [Description("Кур'єр по Києву")]
    CourierKyiv = 3,
    
    [Description("Нова Пошта (кур'єр)")]
    NovaPoshtaCourier = 4,
    
    [Description("Укрпошта")]
    Ukrposhta = 5,
    
    [Description("Кур'єр по Дніпру")]
    CourierDnipro = 6
}
