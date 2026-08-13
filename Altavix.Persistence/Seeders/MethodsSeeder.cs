using Altavix.Domain;
using Altavix.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Persistence.Seeders;

public class MethodsSeeder
{
    private static readonly Dictionary<DeliveryMethodType, string> DeliveryTitles = new()
    {
        { DeliveryMethodType.Pickup, "Самовивіз" },
        { DeliveryMethodType.NovaPoshta, "Нова Пошта (відділення)" },
        { DeliveryMethodType.CourierKyiv, "Кур'єр по Києву" },
        { DeliveryMethodType.NovaPoshtaCourier, "Нова Пошта (кур'єр)" },
        { DeliveryMethodType.Ukrposhta, "Укрпошта" },
        { DeliveryMethodType.CourierDnipro, "Кур'єр по Дніпру" }
    };

    private static readonly Dictionary<PaymentMethodType, string> PaymentTitles = new()
    {
        { PaymentMethodType.CashOnDelivery, "Накладений платіж (при отриманні)" },
        { PaymentMethodType.OnlineCard, "Оплата картою онлайн" },
        { PaymentMethodType.CashlessWithoutVAT, "Безготівковий розрахунок (без ПДВ)" },
        { PaymentMethodType.CryptoUSDT, "Криптовалюта (USDT)" }
    };

    public static void Seed(AltavixDbContext context)
    {
        SeedDeliveryMethods(context);
        SeedPaymentMethods(context);
    }

    private static void SeedDeliveryMethods(AltavixDbContext context)
    {
        var existingDeliveryTypes = context.DeliveryMethods.Select(d => d.Type).ToList();
        var allDeliveryTypes = Enum.GetValues<DeliveryMethodType>().Where(t => t != DeliveryMethodType.Custom);

        foreach (var type in allDeliveryTypes)
        {
            if (!existingDeliveryTypes.Contains(type))
            {
                var title = DeliveryTitles.ContainsKey(type) ? DeliveryTitles[type] : type.ToString();
                context.DeliveryMethods.Add(new DeliveryMethodEntity
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Type = type,
                    IsActive = true,
                    Price = 0
                });
            }
        }
        
        context.SaveChanges();
    }

    private static void SeedPaymentMethods(AltavixDbContext context)
    {
        var existingPaymentTypes = context.PaymentMethods.Select(p => p.Type).ToList();
        var allPaymentTypes = Enum.GetValues<PaymentMethodType>().Where(t => t != PaymentMethodType.Custom);

        foreach (var type in allPaymentTypes)
        {
            if (!existingPaymentTypes.Contains(type))
            {
                var title = PaymentTitles.ContainsKey(type) ? PaymentTitles[type] : type.ToString();
                context.PaymentMethods.Add(new PaymentMethodEntity
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Type = type,
                    IsActive = true
                });
            }
        }

        context.SaveChanges();
    }
}
