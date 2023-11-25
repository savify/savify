using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.Infrastructure.Configuration.DependencyInjection;

internal static class DomainServiceCollectionExtensions
{
    internal static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserNotificationSettingsCounter, UserNotificationSettingsCounter>();

        return services;
    }
}
