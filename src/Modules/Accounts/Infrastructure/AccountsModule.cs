using App.Modules.Accounts.Application.Contracts;
using App.Modules.Accounts.Infrastructure.Configuration.Processing;

namespace App.Modules.Accounts.Infrastructure;

public class AccountsModule : IAccountsModule
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
