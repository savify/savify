using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.DependencyInjection;
using App.Modules.FinanceTracking.Application.Contracts;
using App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;
using App.Modules.FinanceTracking.Infrastructure.Configuration.EventBus;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Quartz;
using App.Modules.FinanceTracking.Infrastructure.Outbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration;

public static class FinanceTrackingModuleCollectionExtensions
{
    public static IServiceCollection AddFinanceTrackingModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "FinanceTracking");
        var connectionString = configuration.GetConnectionString("Savify")!;
        var domainNotificationMap = DomainNotificationsMap.Build();

        services
            .AddDataAccessServices<FinanceTrackingContext>(connectionString)
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

        services.AddScoped<IFinanceTrackingModule, FinanceTrackingModule>();

        return services;
    }
}
