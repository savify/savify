using App.BuildingBlocks.Application.Events;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using App.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;

internal static class OutboxModule
{
    internal static void Configure(IServiceCollection services, BiDictionary<string, Type> domainNotificationsMap)
    {
        services.AddScoped<IOutbox<BanksContext>, Infrastructure.Outbox.Outbox>();

        CheckMappings(domainNotificationsMap);

        services.AddSingleton<IDomainNotificationsMapper<BanksContext>>(_ => new DomainNotificationsMapper<BanksContext>(domainNotificationsMap));
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
