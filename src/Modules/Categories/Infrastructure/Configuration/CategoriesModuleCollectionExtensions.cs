using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Integration;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Infrastructure.Configuration.Domain;
using App.Modules.Categories.Infrastructure.Configuration.EventBus;
using App.Modules.Categories.Infrastructure.Configuration.Integration;
using App.Modules.Categories.Infrastructure.Configuration.Logging;
using App.Modules.Categories.Infrastructure.Configuration.Mediation;
using App.Modules.Categories.Infrastructure.Configuration.Processing;
using App.Modules.Categories.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Categories.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Categories.Infrastructure.Configuration;

public static class CategoriesModuleCollectionExtensions
{
    public static IServiceCollection AddCategoriesModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "Categories");

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            moduleLogger);

        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<ICategoriesModule, CategoriesModule>();

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
        DataAccessModule<CategoriesContext>.Configure(services, connectionString);
        DomainModule.Configure(services);
        LoggingModule.Configure(services, logger);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);
        IntegrationModule.Configure(services);

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
