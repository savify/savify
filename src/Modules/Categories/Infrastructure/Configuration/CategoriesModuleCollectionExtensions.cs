using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Extensions;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Infrastructure.Configuration.EventBus;
using App.Modules.Categories.Infrastructure.Configuration.Extensions;
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
        var connectionString = configuration.GetConnectionString("Savify");
        var domainNotificationMap = GetDomainNotificationMap();

        services
            .AddDataAccessServices<CategoriesContext>(connectionString)
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

        services.AddScoped<ICategoriesModule, CategoriesModule>();

        return services;
    }

    private static BiDictionary<string, Type> GetDomainNotificationMap()
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();

        // domainNotificationsMap.Add(nameof(ExampleDomainEvent), typeof(ExampleNotification));

        return domainNotificationsMap;
    }
}
