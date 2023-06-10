using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Integration;
using App.Modules.Accounts.Application.Contracts;
using App.Modules.Accounts.Infrastructure.Configuration.DataAccess;
using App.Modules.Accounts.Infrastructure.Configuration.Domain;
using App.Modules.Accounts.Infrastructure.Configuration.EventBus;
using App.Modules.Accounts.Infrastructure.Configuration.Logging;
using App.Modules.Accounts.Infrastructure.Configuration.Mediation;
using App.Modules.Accounts.Infrastructure.Configuration.Processing;
using App.Modules.Accounts.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Accounts.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Accounts.Infrastructure.Configuration;

public static class AccountsModuleCollectionExtensions
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "Accounts");

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            moduleLogger);
        
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);
        
        services.AddScoped<IAccountsModule, AccountsModule>();

        return services;
    }

    private static void ConfigureCompositionRoot(
        this IServiceCollection services,
        string connectionString,
        ILogger logger,
        IEventBus? eventBus = null)
    {
        // TODO: move map setup to separate class
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

        AccountsCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
