using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Infrastructure.Configuration.Localization;
using App.Modules.Categories.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Categories.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing;

internal static class ProcessingModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizer>(provider =>
        {
            var localizerFactory = provider.GetRequiredService<ILocalizerFactory>();

            return localizerFactory.Create<CategoriesLocalizationResource>();
        });

        services.AddScoped<IDomainEventsAccessor<CategoriesContext>, DomainEventsAccessor<CategoriesContext>>();
        services.AddScoped<IDomainEventsDispatcher<CategoriesContext>, DomainEventsDispatcher<CategoriesContext>>();
        services.AddScoped<IUnitOfWork<CategoriesContext>, UnitOfWork<CategoriesContext>>();

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.AddScoped<IList<IValidator>>(provider => provider.GetServices<IValidator>().ToList());

        // TODO: uncomment when any command without result will be added
        // services.Decorate(typeof(IRequestHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
        // services.Decorate(typeof(IRequestHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        // services.Decorate(typeof(IRequestHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        // services.Decorate(typeof(IRequestHandler<>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<>));

        // TODO: uncomment when any command with result will be added
        // services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<,>));
    }
}
