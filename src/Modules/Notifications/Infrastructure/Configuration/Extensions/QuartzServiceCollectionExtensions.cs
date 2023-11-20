using App.Modules.Notifications.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.InternalCommands;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.Notifications.Infrastructure.Configuration.Extensions;

internal static class QuartzServiceCollectionExtensions
{
    internal static IServiceCollection AddQuartzServices(this IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessInboxJob>();
        services.AddTransient<IJob, ProcessInternalCommandsJob>();

        return services;
    }
}
