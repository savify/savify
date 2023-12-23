using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.InternalCommands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

internal static class ProcessingServiceCollectionExtensions
{
    internal static IServiceCollection AddProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsAccessor<FinanceTrackingContext>, DomainEventsAccessor<FinanceTrackingContext>>();
        services.AddScoped<IDomainEventsDispatcher<FinanceTrackingContext>, DomainEventsDispatcher<FinanceTrackingContext>>();
        services.AddScoped<IUnitOfWork<FinanceTrackingContext>, UnitOfWork<FinanceTrackingContext>>();

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.Decorate(typeof(IRequestHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.Decorate(typeof(IRequestHandler<>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<>));

        services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<,>));

        return services;
    }
}
