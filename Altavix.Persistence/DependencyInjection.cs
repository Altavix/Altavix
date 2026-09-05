using Altavix.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Altavix.Persistence.Services;

namespace Altavix.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connStr = configuration["ConnectStr"];
        services.AddDbContext<AltavixDbContext>(options =>
        {
            options.UseSqlServer(connStr);
        });

        services.AddScoped<Altavix.Domain.Repositories.IUnitOfWork, Repositories.UnitOfWork>();
        services.AddScoped<IAltavixDbContext>(provider =>
            provider.GetRequiredService<AltavixDbContext>());
        services.AddScoped<IDbConnectionFactory, Factories.DbConnectionFactory>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped(typeof(Altavix.Domain.Repositories.IBaseRepository<>), typeof(Repositories.BaseRepository<>));
        var repositoryTypes = typeof(DependencyInjection).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository") && t.Name != "BaseRepository`1");

        foreach (var type in repositoryTypes)
        {
            var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == $"I{type.Name}");
            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, type);
            }
        }

        services.AddScoped<IDbConnectionFactory, Factories.DbConnectionFactory>();

        return services;
    }
}