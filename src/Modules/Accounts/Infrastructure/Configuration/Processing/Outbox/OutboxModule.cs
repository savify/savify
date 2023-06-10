using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Application.Outbox;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing.Outbox;

internal static class OutboxModule
{
    internal static void Configure(IServiceCollection services, BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox, Infrastructure.Outbox.Outbox>();
        
        CheckMappings(domainNotificationsMap);

        services.AddSingleton<IDomainNotificationsMapper>(_ => new DomainNotificationsMapper(domainNotificationsMap));
    }
    
    private static void CheckMappings(BiDictionary<string, Type> domainNotificationsMap)
    {
        var domainEventNotifications = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IDomainEventNotification)))
            .ToList();

        List<Type> notMappedNotifications = new List<Type>();
        foreach (var domainEventNotification in domainEventNotifications)
        {
            domainNotificationsMap.TryGetBySecond(domainEventNotification, out var name);

            if (name == null)
            {
                notMappedNotifications.Add(domainEventNotification);
            }
        }

        if (notMappedNotifications.Any())
        {
            throw new ApplicationException($"Domain Event Notifications {notMappedNotifications.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
        }
    }
}
