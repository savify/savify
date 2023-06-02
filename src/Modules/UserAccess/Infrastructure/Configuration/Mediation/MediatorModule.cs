using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Mediation;

internal static class MediatorModule
{
    public static void Configure(IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(Assemblies.Application, Assemblies.Infrastructure));

        var mediatorOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
            typeof(IValidator<>)
        };

        foreach (var mediatorOpenType in mediatorOpenTypes)
        {
            services.Scan(scan => scan
                .FromAssemblies(Assemblies.Application, Assemblies.Infrastructure)
                .AddClasses(classes => classes.AssignableTo(mediatorOpenType))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
    }
}
