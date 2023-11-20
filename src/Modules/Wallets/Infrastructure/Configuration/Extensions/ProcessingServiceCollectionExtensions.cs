using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Wallets.Infrastructure.Configuration.Extensions;

internal static class ProcessingServiceCollectionExtensions
{
    internal static IServiceCollection AddProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsAccessor<WalletsContext>, DomainEventsAccessor<WalletsContext>>();
        services.AddScoped<IDomainEventsDispatcher<WalletsContext>, DomainEventsDispatcher<WalletsContext>>();
        services.AddScoped<IUnitOfWork<WalletsContext>, UnitOfWork<WalletsContext>>();

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.AddScoped<IList<IValidator>>(provider => provider.GetServices<IValidator>().ToList());

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
