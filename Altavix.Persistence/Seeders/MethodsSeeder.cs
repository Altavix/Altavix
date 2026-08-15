using System.ComponentModel;
using System.Reflection;
using Altavix.Domain;
using Altavix.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Persistence.Seeders;

public class MethodsSeeder
{
    public static void Seed(AltavixDbContext context)
    {
        SeedDeliveryMethods(context);
        SeedPaymentMethods(context);
    }

    private static string GetDescription(Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = field?.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }

    private static void SeedDeliveryMethods(AltavixDbContext context)
    {
        var existingDeliveryTypes = context.DeliveryMethods.Select(d => d.Type).ToList();
        var allDeliveryTypes = Enum.GetValues<DeliveryMethodType>().Where(t => t != DeliveryMethodType.Custom);

        foreach (var type in allDeliveryTypes)
        {
            if (!existingDeliveryTypes.Contains(type))
            {
                var title = GetDescription(type);
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
                var title = GetDescription(type);
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
