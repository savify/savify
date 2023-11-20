using App.BuildingBlocks.Infrastructure;


namespace App.Modules.Wallets.Infrastructure.Outbox;

internal static class DomainNotificationsMap
{
    internal static BiDictionary<string, Type> Build()
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

        return domainNotificationsMap;
    }
}
