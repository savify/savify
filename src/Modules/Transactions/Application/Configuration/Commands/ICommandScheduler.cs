using App.Modules.Transactions.Application.Contracts;

namespace App.Modules.Transactions.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}
