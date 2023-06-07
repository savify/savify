using App.Modules.Notifications.Domain.UserNotificationSettings;
using App.Modules.Notifications.Infrastructure.Domain.UserNotificationSettings;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.Infrastructure.Configuration.Domain;

internal static class DomainModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddScoped<IUserNotificationSettingsCounter, UserNotificationSettingsCounter>();
    }
}
