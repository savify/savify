using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration.Outbox;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;

internal static class OutboxServiceCollectionExtensions
{
    internal static IServiceCollection AddOutboxServices(this IServiceCollection services, BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox<CategoriesContext>, Infrastructure.Outbox.Outbox>();

        DomainNotificationMappingValidator.Validate(domainNotificationsMap, Assemblies.Application);

        services.AddSingleton<IDomainNotificationsMapper<CategoriesContext>>(_ => new DomainNotificationsMapper<CategoriesContext>(domainNotificationsMap));
        services.AddScoped<OutboxCommandProcessor<CategoriesContext>>();

        return services;
    }
}
