using System.Collections.Specialized;
using App.BuildingBlocks.Infrastructure.Configuration.Quartz;
using App.Modules.Transactions.Infrastructure.Configuration.Processing.Inbox;
using App.Modules.Transactions.Infrastructure.Configuration.Processing.InternalCommands;
using App.Modules.Transactions.Infrastructure.Configuration.Processing.Outbox;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Serilog;

namespace App.Modules.Transactions.Infrastructure.Configuration.Quartz;

internal static class QuartzInitialization
{
    private static IScheduler _scheduler;

    internal static void Initialize(ILogger logger)
    {
        logger.Information("Quartz starting...");

        _scheduler = CreateScheduler();

        LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));

        _scheduler.Start().GetAwaiter().GetResult();

        ScheduleProcessOutboxJob(_scheduler);
        ScheduleProcessInboxJob(_scheduler);
        ScheduleProcessInternalCommandJob(_scheduler);

        logger.Information("Quartz started");
    }

    internal static void StopQuartz()
    {
        _scheduler.Shutdown();
    }

    private static IScheduler CreateScheduler()
    {
        var schedulerConfiguration = new NameValueCollection();
        schedulerConfiguration.Add("quartz.scheduler.instanceName", "Savify");

        ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
        var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        return scheduler;
    }

    private static void ScheduleProcessOutboxJob(IScheduler scheduler)
    {
        var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
        var outboxProcessingTrigger = TriggerBuilder
            .Create()
            .StartNow()
            .WithCronSchedule("0/5 * * ? * *") // every 5 seconds
            .Build();

        scheduler.ScheduleJob(processOutboxJob, outboxProcessingTrigger).GetAwaiter().GetResult();
    }

    private static void ScheduleProcessInboxJob(IScheduler scheduler)
    {
        var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();
        var outboxProcessingTrigger = TriggerBuilder
            .Create()
            .StartNow()
            .WithCronSchedule("0/5 * * ? * *") // every 5 seconds
            .Build();

        scheduler.ScheduleJob(processInboxJob, outboxProcessingTrigger).GetAwaiter().GetResult();
    }

    private static void ScheduleProcessInternalCommandJob(IScheduler scheduler)
    {
        var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();
        var commandsProcessingTrigger = TriggerBuilder
            .Create()
            .StartNow()
            .WithCronSchedule("0/5 * * ? * *") // every 5 seconds
            .Build();

        scheduler.ScheduleJob(processInternalCommandsJob, commandsProcessingTrigger).GetAwaiter().GetResult();
    }
}
