using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}
