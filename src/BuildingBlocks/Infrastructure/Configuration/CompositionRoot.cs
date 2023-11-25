using Microsoft.Extensions.DependencyInjection;

namespace App.BuildingBlocks.Infrastructure.Configuration;

public static class CompositionRoot
{
    private static IServiceProvider _serviceProvider;

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IServiceScope BeginScope()
    {
        return _serviceProvider.CreateScope();
    }
}
