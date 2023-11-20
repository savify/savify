using App.Modules.Wallets.Application.Contracts;
using App.Modules.Wallets.Infrastructure.Configuration.Processing;

namespace App.Modules.Wallets.Infrastructure;

public class WalletsModule : IWalletsModule
{
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
