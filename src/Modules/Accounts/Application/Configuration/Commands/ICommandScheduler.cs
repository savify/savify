using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}
