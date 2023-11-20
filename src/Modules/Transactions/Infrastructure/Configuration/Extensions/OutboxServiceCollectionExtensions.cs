using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.Extensions;

internal static class OutboxServiceCollectionExtensions
{
    internal static IServiceCollection AddOutboxServices(this IServiceCollection services, BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox<TransactionsContext>, Infrastructure.Outbox.Outbox>();

        DomainNotificationMappingValidator.Validate(domainNotificationsMap, Assemblies.Application);

        services.AddSingleton<IDomainNotificationsMapper<TransactionsContext>>(_ => new DomainNotificationsMapper<TransactionsContext>(domainNotificationsMap));

        return services;
    }
}
