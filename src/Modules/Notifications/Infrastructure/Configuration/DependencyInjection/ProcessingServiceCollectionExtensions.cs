using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.InternalCommands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.Infrastructure.Configuration.DependencyInjection;

internal static class ProcessingServiceCollectionExtensions
{
    internal static IServiceCollection AddProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsAccessor<NotificationsContext>, DomainEventsAccessor<NotificationsContext>>();
        services.AddScoped<IDomainEventsDispatcher<NotificationsContext>, App.Modules.Notifications.Infrastructure.DomainEventsDispatching.DomainEventsDispatcher<NotificationsContext>>();
        services.AddScoped<IUnitOfWork<NotificationsContext>, UnitOfWork<NotificationsContext>>();

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.Decorate(typeof(IRequestHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<>));

        // TODO: uncomment when any command with result will be added
        // services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<,>));

        return services;
    }
}
