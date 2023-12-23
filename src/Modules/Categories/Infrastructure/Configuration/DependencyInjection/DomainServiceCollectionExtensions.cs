using App.Modules.Categories.Domain.Categories;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;
using App.Modules.Categories.Infrastructure.Domain.Categories;
using App.Modules.Categories.Infrastructure.Domain.CategoriesSynchronisationProcessing;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoriesSynchronisationService, CategoriesSynchronisationService>();
        services.AddScoped<ICategoriesCounter, CategoriesCounter>();

        return services;
    }
}
