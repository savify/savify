using Quartz;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands;

[DisallowConcurrentExecution]
public class ProcessInternalCommandsJob: IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.Execute(new ProcessInternalCommandsCommand());
    }
}
