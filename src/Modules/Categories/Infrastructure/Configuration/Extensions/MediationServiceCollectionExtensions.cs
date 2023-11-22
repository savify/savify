using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.Extensions;

internal static class MediationServiceCollectionExtensions
{
    internal static IServiceCollection AddMediationServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(Assemblies.Application, Assemblies.Infrastructure));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        return services;
    }
}
