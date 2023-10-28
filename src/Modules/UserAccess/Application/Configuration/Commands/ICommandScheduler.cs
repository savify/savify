using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);
}
