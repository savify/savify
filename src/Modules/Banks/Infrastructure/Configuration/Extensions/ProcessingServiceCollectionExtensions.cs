using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Extensions;

internal static class ProcessingServiceCollectionExtensions
{
    internal static IServiceCollection AddProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsAccessor<BanksContext>, DomainEventsAccessor<BanksContext>>();
        services.AddScoped<IDomainEventsDispatcher<BanksContext>, DomainEventsDispatcher<BanksContext>>();
        services.AddScoped<IUnitOfWork<BanksContext>, UnitOfWork<BanksContext>>();

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
