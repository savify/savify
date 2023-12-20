using App.BuildingBlocks.Infrastructure.Configuration;
using App.BuildingBlocks.Infrastructure.Configuration.DependencyInjection;
using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Infrastructure.Authentication;
using App.Modules.UserAccess.Infrastructure.Configuration.DependencyInjection;
using App.Modules.UserAccess.Infrastructure.Configuration.EventBus;
using App.Modules.UserAccess.Infrastructure.Configuration.Quartz;
using App.Modules.UserAccess.Infrastructure.Outbox;
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
        var connectionString = configuration.GetConnectionString("Savify");
        var domainNotificationMap = DomainNotificationsMap.Build();
        var authenticationConfiguration = configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();

        services
            .AddAuthenticationServices(authenticationConfiguration!)
            .AddDataAccessServices<UserAccessContext>(connectionString!)
            .AddDomainServices()
            .AddLocalizationServices()
            .AddLoggingServices()
            .AddMediatRForAssemblies(Assemblies.Application, Assemblies.Infrastructure)
            .AddOutboxServices(domainNotificationMap)
            .AddProcessingServices()
            .AddQuartzServices();

        CompositionRoot.SetServiceProvider(services.BuildServiceProvider());
        QuartzInitialization.Initialize(moduleLogger);
        EventBusInitialization.Initialize(moduleLogger);

        services.AddScoped<IUserAccessModule, UserAccessModule>();

        return services;
    }
}
