using Altavix.Domain;
using Altavix.Domain.Enums;
using Altavix.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;

namespace Altavix.Persistence;

public class DbInitializer
{
    public static void Initialize(AltavixDbContext context)
    {
        context.Database.Migrate();

        MethodsSeeder.Seed(context);
    }
}