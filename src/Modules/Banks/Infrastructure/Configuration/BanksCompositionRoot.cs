using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration;

public class BanksCompositionRoot
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
