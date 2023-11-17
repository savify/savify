using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Integration;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure.Configuration.Domain;
using App.Modules.Banks.Infrastructure.Configuration.EventBus;
using App.Modules.Banks.Infrastructure.Configuration.Integration;
using App.Modules.Banks.Infrastructure.Configuration.Logging;
using App.Modules.Banks.Infrastructure.Configuration.Mediation;
using App.Modules.Banks.Infrastructure.Configuration.Processing;
using App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Banks.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Banks.Infrastructure.Configuration;

public static class BanksModuleCollectionExtensions
{
    public static IServiceCollection AddBanksModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger,
        bool isProduction)
    {
        var moduleLogger = logger.ForContext("Module", "Banks");

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            moduleLogger,
            isProduction);

        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<IBanksModule, BanksModule>();

        return services;
    }

    private static void ConfigureCompositionRoot(
        this IServiceCollection services,
        string connectionString,
        ILogger logger,
        bool isProduction,
        IEventBus? eventBus = null)
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

        DataAccessModule<BanksContext>.Configure(services, connectionString);
        OutboxModule.Configure(services, domainNotificationsMap);
        DomainModule.Configure(services);
        LoggingModule.Configure(services, logger);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);
        IntegrationModule.Configure(services, isProduction);

        BanksCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
