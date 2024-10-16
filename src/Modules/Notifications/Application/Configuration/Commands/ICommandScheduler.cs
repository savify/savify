using App.Modules.Notifications.Application.Contracts;

namespace App.Modules.Notifications.Application.Configuration.Commands;

public interface ICommandScheduler
{
    Task EnqueueAsync(ICommand command);
}
