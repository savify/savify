using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Configuration.Extensions;

public static class MediationServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRForAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));

        return services;
    }
}
