using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Configuration.DependencyInjection;

public static class MediationServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRForAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));

        var mediatorOpenTypes = new[]
        {
            typeof(IValidator<>)
        };

        foreach (var mediatorOpenType in mediatorOpenTypes)
        {
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(mediatorOpenType))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        return services;
    }
}
