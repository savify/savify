using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Localization;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Infrastructure.Configuration.Localization;
using App.Modules.Banks.Infrastructure.Configuration.Processing.Decorators;
using App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing;

internal static class ProcessingModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddSingleton<IStringLocalizer>(provider =>
        {
            var localizerFactory = provider.GetRequiredService<ILocalizerFactory>();

            return localizerFactory.Create<BanksLocalizationResource>();
        });

        services.AddScoped<IDomainEventsAccessor>(provider =>
        {
            return new DomainEventsAccessor(provider.GetRequiredService<BanksContext>());
        });
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddScoped<IUnitOfWork>(provider =>
        {
            return new UnitOfWork(
                provider.GetRequiredService<BanksContext>(),
                provider.GetRequiredService<IDomainEventsDispatcher>());
        });

        services.AddScoped<ICommandScheduler, CommandScheduler>();

        services.AddScoped<IList<IValidator>>(provider => provider.GetServices<IValidator>().ToList());

        // TODO: uncomment when any command/query will be added
        // services.Decorate(typeof(IRequestHandler<,>), typeof(UnitOfWorkCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingCommandHandlerDecorator<,>));
        // services.Decorate(typeof(IRequestHandler<,>), typeof(BusinessRuleExceptionLocalizationCommandHandlerDecorator<,>));
    }
}
