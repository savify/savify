using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Extensions;
using App.Modules.Transactions.Application.Contracts;
using App.Modules.Transactions.Infrastructure.Configuration.EventBus;
using App.Modules.Transactions.Infrastructure.Configuration.Extensions;
using App.Modules.Transactions.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Transactions.Infrastructure.Configuration;

public static class TransactionsModuleCollectionExtensions
{
    public static IServiceCollection AddTransactionsModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "Transactions");
        var connectionString = configuration.GetConnectionString("Savify");
        var domainNotificationMap = GetDomainNotificationMap();

        services
            .AddDataAccessServices<TransactionsContext>(connectionString)
            .AddDomainServices()
            .AddIntegrationServices()
            .AddLocalizationServices()
            .AddLoggingServices()
            .AddMediationServices()
            .AddOutboxServices(domainNotificationMap)
            .AddProcessingServices()
            .AddQuartzServices();

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<ITransactionsModule, TransactionsModule>();

        return services;
    }

    private static BiDictionary<string, Type> GetDomainNotificationMap()
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

        return domainNotificationsMap;
    }
}
