using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Extensions;

internal static class OutboxServiceCollectionExtensions
{
    internal static IServiceCollection AddOutboxServices(this IServiceCollection services, BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox<UserAccessContext>, Infrastructure.Outbox.Outbox>();

        DomainNotificationMappingValidator.Validate(domainNotificationsMap, Assemblies.Application);

        services.AddSingleton<IDomainNotificationsMapper<UserAccessContext>>(_ => new DomainNotificationsMapper<UserAccessContext>(domainNotificationsMap));

        return services;
    }
}
