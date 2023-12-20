using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Infrastructure.Domain.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesCounter, CategoriesCounter>();

        return services;
    }
}
