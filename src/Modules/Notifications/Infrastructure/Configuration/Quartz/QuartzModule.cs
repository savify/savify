using App.Modules.Notifications.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.InternalCommands;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.Notifications.Infrastructure.Configuration.Quartz;

internal static class QuartzModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessInboxJob>();
        services.AddTransient<IJob, ProcessInternalCommandsJob>();
    }
}
