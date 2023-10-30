using App.Modules.Notifications.Application.Contracts;
using App.Modules.Notifications.Infrastructure.Configuration.Processing;

namespace App.Modules.Notifications.Infrastructure;

public class NotificationsModule : INotificationsModule
{
    public static string DatabaseSchemaName => "notifications";

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        return await QueryExecutor.Execute(query);
    }
}
