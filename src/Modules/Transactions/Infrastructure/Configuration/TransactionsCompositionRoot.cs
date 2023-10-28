using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration;

public class TransactionsCompositionRoot
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
