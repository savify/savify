using App.Modules.Banks.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;
using App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Banks.Infrastructure.Configuration.Processing.RecurringCommands;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.Banks.Infrastructure.Configuration.DependencyInjection;

internal static class QuartzServiceCollectionExtensions
{
    internal static IServiceCollection AddQuartzServices(this IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessOutboxJob>();
        services.AddTransient<IJob, ProcessInboxJob>();
        services.AddTransient<IJob, ProcessInternalCommandsJob>();
        services.AddTransient<IJob, BanksSynchronisationJob>();

        return services;
    }
}
