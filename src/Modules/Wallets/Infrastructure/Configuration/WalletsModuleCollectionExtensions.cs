using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.DependencyInjection;
using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Infrastructure.Configuration.EventBus;
using App.Modules.Wallets.Infrastructure.Configuration.DependencyInjection;
using App.Modules.Wallets.Infrastructure.Configuration.Quartz;
using App.Modules.Wallets.Infrastructure.Outbox;
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
        var domainNotificationMap = DomainNotificationsMap.Build();

        services
            .AddDataAccessServices<WalletsContext>(connectionString)
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

        services.AddScoped<IWalletsModule, WalletsModule>();

        return services;
    }
}
