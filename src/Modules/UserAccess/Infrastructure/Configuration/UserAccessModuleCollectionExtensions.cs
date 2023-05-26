using App.BuildingBlocks.Infrastructure;
using App.BuildingBlocks.Infrastructure.Authentication;
using App.BuildingBlocks.Infrastructure.Emails;
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

        ConfigureCompositionRoot(services, configuration, moduleLogger);
        
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<IUserAccessModule, UserAccessModule>();

        return services;
    }
    
    private static void ConfigureCompositionRoot(this IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        var domainNotificationsMap = new BiDictionary<string, Type>();
        
        domainNotificationsMap.Add(nameof(UserCreatedDomainEvent), typeof(UserCreatedNotification));
        
        OutboxModule.Configure(services, domainNotificationsMap);
        AuthenticationModule.Configure(services, configuration.GetSection("Authentication").Get<AuthenticationConfiguration>());
        DataAccessModule.Configure(services, configuration.GetConnectionString("Savify"));
        DomainModule.Configure(services);
        EmailModule.Configure(services, configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
        LoggingModule.Configure(services, logger);
        EventBusModule.Configure(services);
        QuartzModule.Configure(services);
        MediatorModule.Configure(services);
        ProcessingModule.Configure(services);

        UserAccessCompositionRoot.SetServiceProvider(services.BuildServiceProvider());
    }
}
