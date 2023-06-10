using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}
