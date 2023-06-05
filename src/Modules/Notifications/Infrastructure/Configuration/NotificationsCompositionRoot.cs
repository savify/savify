using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Notifications.Infrastructure.Configuration;

public class NotificationsCompositionRoot
{
    private static IServiceProvider _serviceProvider;
    
    internal static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal static IServiceScope BeginScope()
    {
        return _serviceProvider.CreateScope();
    }
}
