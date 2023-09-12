using App.Modules.Banks.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Banks.Infrastructure.Configuration.Processing.InternalCommands;
using App.Modules.Banks.Infrastructure.Configuration.Processing.Outbox;
using App.Modules.Banks.Infrastructure.Configuration.Processing.RecurringCommands;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace App.Modules.Banks.Infrastructure.Configuration.Quartz;

internal static class QuartzModule
{
    internal static void Configure(IServiceCollection services)
    {
        services.AddTransient<IJob, ProcessOutboxJob>();
        services.AddTransient<IJob, ProcessInboxJob>();
        services.AddTransient<IJob, ProcessInternalCommandsJob>();
        services.AddTransient<IJob, BanksSynchronisationJob>();
    }
}
