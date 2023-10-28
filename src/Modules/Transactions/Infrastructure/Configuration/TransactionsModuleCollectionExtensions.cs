using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Integration;
using App.Modules.Transactions.Application.Contracts;
using App.Modules.Transactions.Infrastructure.Configuration.DataAccess;
using App.Modules.Transactions.Infrastructure.Configuration.Domain;
using App.Modules.Transactions.Infrastructure.Configuration.EventBus;
using App.Modules.Transactions.Infrastructure.Configuration.Integration;
using App.Modules.Transactions.Infrastructure.Configuration.Logging;
using App.Modules.Transactions.Infrastructure.Configuration.Mediation;
using App.Modules.Transactions.Infrastructure.Configuration.Processing;
using App.Modules.Transactions.Infrastructure.Configuration.Processing.Outbox;
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

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            moduleLogger);

        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<ITransactionsModule, TransactionsModule>();

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
        IntegrationModule.Configure(services);

        TransactionsCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
