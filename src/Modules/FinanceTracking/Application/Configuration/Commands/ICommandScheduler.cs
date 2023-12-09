using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);
}
