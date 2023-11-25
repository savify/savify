using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Categories.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class ProcessingServiceCollectionExtensions
{
    internal static IServiceCollection AddProcessingServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventsAccessor<CategoriesContext>, DomainEventsAccessor<CategoriesContext>>();
        services.AddScoped<IDomainEventsDispatcher<CategoriesContext>, DomainEventsDispatcher<CategoriesContext>>();
        services.AddScoped<IUnitOfWork<CategoriesContext>, UnitOfWork<CategoriesContext>>();

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.AddScoped<IList<IValidator>>(provider => provider.GetServices<IValidator>().ToList());

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
