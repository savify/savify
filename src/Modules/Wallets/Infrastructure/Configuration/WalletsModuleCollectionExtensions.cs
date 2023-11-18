using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Integration;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Infrastructure.Configuration.Domain;
using App.Modules.Wallets.Infrastructure.Configuration.EventBus;
using App.Modules.Wallets.Infrastructure.Configuration.Integration;
using App.Modules.Wallets.Infrastructure.Configuration.Logging;
using App.Modules.Wallets.Infrastructure.Configuration.Mediation;
using App.Modules.Wallets.Infrastructure.Configuration.Processing;
using App.Modules.Wallets.Infrastructure.Configuration.Processing.Outbox;
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

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            moduleLogger);

        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<IWalletsModule, WalletsModule>();

        return services;
    }

    private static void ConfigureCompositionRoot(
        this IServiceCollection services,
        string connectionString,
        ILogger logger,
        IEventBus? eventBus = null)
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

        DataAccessModule<WalletsContext>.Configure(services, connectionString);
        OutboxModule.Configure(services, domainNotificationsMap);
        DomainModule.Configure(services);
        LoggingModule.Configure(services, logger);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);
        IntegrationModule.Configure(services);

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
