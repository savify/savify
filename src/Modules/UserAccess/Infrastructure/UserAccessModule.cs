using App.Modules.UserAccess.Application.Contracts;
using App.Modules.UserAccess.Infrastructure.Configuration.Processing;

namespace App.Modules.UserAccess.Infrastructure;

public class UserAccessModule : IUserAccessModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        return await QueryExecutor.Execute(query);
    }
}
