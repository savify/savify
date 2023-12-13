using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.DependencyInjection;

public static class NamingConventionsServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkNamingConventions(this IServiceCollection services)
    {
        new EntityFrameworkServicesBuilder(services).TryAdd<IConventionSetPlugin, NamingConventionSetPlugin>();

        return services;
    }
}
