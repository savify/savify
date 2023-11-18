using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Infrastructure.Configuration.Localization;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Modules.Wallets.Infrastructure.Configuration.Processing;

internal static class ProcessingModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizer>(provider =>
        {
            var localizerFactory = provider.GetRequiredService<ILocalizerFactory>();

            return localizerFactory.Create<WalletsLocalizationResource>();
        });

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
    }
}
