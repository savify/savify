using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.Extensions;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // register domain services here

        return services;
    }
}
