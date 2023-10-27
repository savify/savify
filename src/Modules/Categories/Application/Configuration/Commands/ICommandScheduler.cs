using App.Modules.Categories.Application.Contracts;

namespace App.Modules.Categories.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync<T>(ICommand<T> command);
}
