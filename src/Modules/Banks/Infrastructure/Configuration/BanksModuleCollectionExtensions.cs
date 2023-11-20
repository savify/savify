using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Extensions;
using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure.Configuration.EventBus;
using App.Modules.Banks.Infrastructure.Configuration.Extensions;
using App.Modules.Banks.Infrastructure.Configuration.Quartz;
using App.Modules.Banks.Infrastructure.Outbox;
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
        var connectionString = configuration.GetConnectionString("Savify");
        var domainNotificationMap = DomainNotificationsMap.Build();

        services
            .AddDataAccessServices<BanksContext>(connectionString)
            .AddDomainServices()
            .AddIntegrationServices(isProduction)
            .AddLocalizationServices()
            .AddLoggingServices()
            .AddMediationServices()
            .AddOutboxServices(domainNotificationMap)
            .AddProcessingServices()
            .AddQuartzServices();

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<IBanksModule, BanksModule>();

        return services;
    }
}
