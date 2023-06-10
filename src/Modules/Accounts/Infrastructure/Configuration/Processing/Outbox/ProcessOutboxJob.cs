using Quartz;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.Execute(new ProcessOutboxCommand());
    }
}
