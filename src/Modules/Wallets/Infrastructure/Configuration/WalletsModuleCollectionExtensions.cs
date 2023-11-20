using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Extensions;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Infrastructure.Configuration.EventBus;
using App.Modules.Wallets.Infrastructure.Configuration.Extensions;
using App.Modules.Wallets.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Wallets.Infrastructure.Configuration;

public static class WalletsModuleCollectionExtensions
{
    public static IServiceCollection AddWalletsModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "Wallets");
        var connectionString = configuration.GetConnectionString("Savify");
        var domainNotificationMap = GetDomainNotificationMap();

        services
            .AddDataAccessServices<WalletsContext>(connectionString)
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

        services.AddScoped<IWalletsModule, WalletsModule>();

        return services;
    }

    private static BiDictionary<string, Type> GetDomainNotificationMap()
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

        return domainNotificationsMap;
    }
}
