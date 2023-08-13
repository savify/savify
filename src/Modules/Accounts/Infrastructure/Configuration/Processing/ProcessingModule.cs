using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Accounts.Infrastructure.Configuration.Localization;
using App.Modules.Accounts.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Accounts.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing;

internal static class ProcessingModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizer>(provider =>
        {
            var localizerFactory = provider.GetRequiredService<ILocalizerFactory>();

            return localizerFactory.Create<AccountsLocalizationResource>();
        });
        
        services.AddScoped<IDomainEventsAccessor>(provider =>
        {
            return new DomainEventsAccessor(provider.GetRequiredService<AccountsContext>());
        });
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddScoped<IUnitOfWork>(provider =>
        {
            return new UnitOfWork(
                provider.GetRequiredService<AccountsContext>(),
                provider.GetRequiredService<IDomainEventsDispatcher>());
        });

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.AddScoped<IList<IValidator>>(provider => provider.GetServices<IValidator>().ToList());

        services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));
        services.Decorate(typeof(IRequestHandler<,>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<,>));
    }
}
