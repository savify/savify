using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Integration;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure.Configuration.DataAccess;
using App.Modules.Banks.Infrastructure.Configuration.Domain;
using App.Modules.Banks.Infrastructure.Configuration.EventBus;
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
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "Banks");

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            moduleLogger);

        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<IBanksModule, BanksModule>();

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

        OutboxModule.Configure(services, domainNotificationsMap);
        DataAccessModule.Configure(services, connectionString);
        DomainModule.Configure(services);
        LoggingModule.Configure(services, logger);
        EventBusModule.Configure(services, eventBus);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);

        BanksCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
