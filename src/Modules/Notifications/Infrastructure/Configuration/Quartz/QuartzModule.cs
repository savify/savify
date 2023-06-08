using App.Modules.Notifications.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Notifications.Infrastructure.Configuration.Processing.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.Notifications.Infrastructure.Configuration.Quartz;

internal static class QuartzModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessOutboxJob>();
        services.AddTransient<IJob, ProcessInboxJob>();
    }
}
