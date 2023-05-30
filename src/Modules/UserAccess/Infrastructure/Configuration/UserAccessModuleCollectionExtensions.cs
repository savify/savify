using App.BuildingBlocks.Application.Emails;
using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Authentication;
using App.BuildingBlocks.Infrastructure.Emails;
using App.BuildingBlocks.Integration;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Domain.Users.Events;
using App.Modules.UserAccess.Infrastructure.Configuration.Authentication;
using App.Modules.UserAccess.Infrastructure.Configuration.DataAccess;
using App.Modules.UserAccess.Infrastructure.Configuration.Domain;
using App.Modules.UserAccess.Infrastructure.Configuration.Email;
using App.Modules.UserAccess.Infrastructure.Configuration.EventBus;
using App.Modules.UserAccess.Infrastructure.Configuration.Logging;
using App.Modules.UserAccess.Infrastructure.Configuration.Mediation;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.UserAccess.Infrastructure.Configuration.Quartz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace App.Modules.UserAccess.Infrastructure.Configuration;

public static class UserAccessModuleCollectionExtensions
{
    public static IServiceCollection AddUserAccessModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var moduleLogger = logger.ForContext("Module", "UserAccess");

        ConfigureCompositionRoot(
            services,
            configuration.GetConnectionString("Savify"),
            configuration.GetSection("Authentication").Get<AuthenticationConfiguration>(),
            configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>(),
            moduleLogger);

        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);
        
        services.AddScoped<IUserAccessModule, UserAccessModule>();

        return services;
    }

    public static IServiceCollection AddTestUserAccessModule(
        this IServiceCollection services,
        string connectionString,
        ILogger logger,
        IEmailSender emailSender,
        IEmailMessageFactory emailMessageFactory,
        IEventBus? eventBus
        )
    {
        ConfigureCompositionRoot(
            services,
            connectionString,
            new AuthenticationConfiguration(),
            new EmailConfiguration(),
            logger,
            emailSender,
            emailMessageFactory,
            eventBus);
        
        services.AddScoped<IUserAccessModule, UserAccessModule>();
        
        return services;
    }
    
    private static void ConfigureCompositionRoot(
        this IServiceCollection services,
        string connectionString,
        AuthenticationConfiguration authenticationConfiguration,
        EmailConfiguration emailConfiguration,
        ILogger logger,
        IEmailSender? emailSender = null,
        IEmailMessageFactory? emailMessageFactory = null,
        IEventBus? eventBus = null)
    {
        // TODO: move map setup to separate class
        var domainNotificationsMap = new BiDictionary<string, Type>();
        
        domainNotificationsMap.Add(nameof(UserCreatedDomainEvent), typeof(UserCreatedNotification));
        
        OutboxModule.Configure(services, domainNotificationsMap);
        AuthenticationModule.Configure(services, authenticationConfiguration);
        DataAccessModule.Configure(services, connectionString);
        DomainModule.Configure(services);
        EmailModule.Configure(services, emailConfiguration, emailSender, emailMessageFactory);
        LoggingModule.Configure(services, logger);
        EventBusModule.Configure(services, eventBus);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);

        UserAccessCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
