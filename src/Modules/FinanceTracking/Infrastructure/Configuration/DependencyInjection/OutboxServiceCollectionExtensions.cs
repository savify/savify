using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

internal static class OutboxServiceCollectionExtensions
{
    internal static IServiceCollection AddOutboxServices(this IServiceCollection services, BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox<FinanceTrackingContext>, Infrastructure.Outbox.Outbox>();

        DomainNotificationMappingValidator.Validate(domainNotificationsMap, Assemblies.Application);

        services.AddSingleton<IDomainNotificationsMapper<FinanceTrackingContext>>(_ => new DomainNotificationsMapper<FinanceTrackingContext>(domainNotificationsMap));
        services.AddScoped<OutboxCommandProcessor<FinanceTrackingContext>>();

        return services;
    }
}
