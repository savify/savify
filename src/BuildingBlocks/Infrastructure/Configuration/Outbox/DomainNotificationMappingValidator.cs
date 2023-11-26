using System.Reflection;
using App.BuildingBlocks.Application.Events;

namespace App.BuildingBlocks.Infrastructure.Configuration.Outbox;

public static class DomainNotificationMappingValidator
{
    public static void Validate(BiDictionary<string, Type> domainNotificationsMap, Assembly assembly)
    {
        var domainEventNotifications = assembly
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IDomainEventNotification)))
            .ToList();

        var notMappedNotifications = new List<Type>();
        foreach (var domainEventNotification in domainEventNotifications)
        {
            domainNotificationsMap.TryGetBySecond(domainEventNotification, out var name);

            if (name is null)
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
