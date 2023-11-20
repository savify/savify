using App.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Extensions;

internal static class QuartzServiceCollectionExtensions
{
    internal static IServiceCollection AddQuartzServices(this IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessOutboxJob>();
        services.AddTransient<IJob, ProcessInboxJob>();
        services.AddTransient<IJob, ProcessInternalCommandsJob>();

        return services;
    }
}
