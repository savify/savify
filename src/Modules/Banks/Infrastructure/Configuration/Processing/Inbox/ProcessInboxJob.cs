using Quartz;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Inbox;

[DisallowConcurrentExecution]
public class ProcessInboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.Execute(new ProcessInboxCommand());
    }
}
