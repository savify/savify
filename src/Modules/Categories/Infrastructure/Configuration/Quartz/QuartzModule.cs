using App.Modules.Categories.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Categories.Infrastructure.Configuration.Processing.InternalCommands;
using App.Modules.Categories.Infrastructure.Configuration.Processing.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.Categories.Infrastructure.Configuration.Quartz;

internal static class QuartzModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessOutboxJob>();
        services.AddTransient<IJob, ProcessInboxJob>();
        services.AddTransient<IJob, ProcessInternalCommandsJob>();
    }
}
