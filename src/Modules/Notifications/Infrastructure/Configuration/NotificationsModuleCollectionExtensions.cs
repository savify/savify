using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.Extensions;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Infrastructure.Configuration.EventBus;
using App.Modules.Notifications.Infrastructure.Configuration.Extensions;
using App.Modules.Notifications.Infrastructure.Configuration.Quartz;
using App.Modules.Notifications.Infrastructure.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.Notifications.Infrastructure.Configuration;

public static class NotificationsModuleCollectionExtensions
{
    public static IServiceCollection AddNotificationsModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "Notifications");
        var connectionString = configuration.GetConnectionString("Savify");
        var emailConfiguration = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

        services
            .AddDataAccessServices<NotificationsContext>(connectionString)
            .AddDomainServices()
            .AddEmailingServices(emailConfiguration, moduleLogger)
            .AddLocalizationServices()
            .AddLoggingServices()
            .AddMediationServices()
            .AddProcessingServices()
            .AddQuartzServices();

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<INotificationsModule, NotificationsModule>();

        return services;
    }
}
