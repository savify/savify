using App.BuildingBlocks.Infrastructure;
using App.Modules.Categories.Application.Categories.CreateCategory;
using App.Modules.Categories.Domain.Categories.Events;


namespace App.Modules.Categories.Infrastructure.Outbox;

internal static class DomainNotificationsMap
{
    internal static BiDictionary<string, Type> Build()
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        domainNotificationsMap.Add(nameof(NewCategoryCreatedDomainEvent), typeof(NewCategoryCreatedNotification));

        return domainNotificationsMap;
    }
}
