using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration;

public class UserAccessCompositionRoot
{
    private static IServiceProvider _serviceProvider;

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal static IServiceScope BeginScope()
    {
        return _serviceProvider.CreateScope();
    }
}
