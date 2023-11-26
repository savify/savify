using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Transactions.Application.Configuration.Commands;
using App.Modules.Transactions.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Transactions.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.DependencyInjection;

internal static class ProcessingServiceCollectionExtensions
{
    internal static IServiceCollection AddProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsAccessor<TransactionsContext>, DomainEventsAccessor<TransactionsContext>>();
        services.AddScoped<IDomainEventsDispatcher<TransactionsContext>, DomainEventsDispatcher<TransactionsContext>>();
        services.AddScoped<IUnitOfWork<TransactionsContext>, UnitOfWork<TransactionsContext>>();

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
