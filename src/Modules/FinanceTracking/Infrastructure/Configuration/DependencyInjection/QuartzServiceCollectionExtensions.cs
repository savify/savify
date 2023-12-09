using App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.InternalCommands;
using App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.DependencyInjection;

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
