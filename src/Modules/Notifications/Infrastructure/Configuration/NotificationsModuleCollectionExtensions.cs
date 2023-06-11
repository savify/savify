using App.BuildingBlocks.Integration;
using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Application.Emails;
using App.Modules.Notifications.Infrastructure.Configuration.DataAccess;
using App.Modules.Notifications.Infrastructure.Configuration.Domain;
using App.Modules.Notifications.Infrastructure.Configuration.Email;
using App.Modules.Notifications.Infrastructure.Configuration.EventBus;
using App.Modules.Notifications.Infrastructure.Configuration.Logging;
using App.Modules.Notifications.Infrastructure.Configuration.Mediation;
using App.Modules.Notifications.Infrastructure.Configuration.Processing;
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

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>(),
            moduleLogger);
        
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);
        
        services.AddScoped<INotificationsModule, NotificationsModule>();

        return services;
    }

    private static void ConfigureCompositionRoot(
        this IServiceCollection services,
        string connectionString,
        EmailConfiguration emailConfiguration,
        ILogger logger,
        IEmailSender? emailSender = null,
        IEmailMessageFactory? emailMessageFactory = null,
        IEventBus? eventBus = null)
    {
        DataAccessModule.Configure(services, connectionString);
        DomainModule.Configure(services);
        EmailModule.Configure(services, emailConfiguration, emailSender, emailMessageFactory);
        LoggingModule.Configure(services, logger);
        EventBusModule.Configure(services, eventBus);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);

        NotificationsCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
