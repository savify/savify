using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.DependencyInjection;
using App.Modules.Categories.Application.Contracts;
using App.Modules.Categories.Infrastructure.Configuration.DependencyInjection;
using App.Modules.Categories.Infrastructure.Configuration.EventBus;
using App.Modules.Categories.Infrastructure.Configuration.Quartz;
using App.Modules.Categories.Infrastructure.Outbox;
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
        var connectionString = configuration.GetConnectionString("Savify")!;
        var domainNotificationMap = DomainNotificationsMap.Build();

        services
            .AddDataAccessServices<CategoriesContext>(connectionString)
            .AddDomainServices()
            .AddIntegrationServices()
            .AddLocalizationServices()
            .AddLoggingServices()
            .AddMediatRForAssemblies(Assemblies.Application, Assemblies.Infrastructure)
            .AddOutboxServices(domainNotificationMap)
            .AddProcessingServices()
            .AddQuartzServices();

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<ICategoriesModule, CategoriesModule>();

        return services;
    }
}
