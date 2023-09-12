using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using Quartz;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.RecurringCommands;

public class BanksSynchronisationJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandExecutor.Execute(new SynchroniseBanksRecurringCommand());
    }
}
