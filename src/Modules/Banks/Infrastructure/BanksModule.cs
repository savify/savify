using App.Modules.Banks.Application.Contracts;
using App.Modules.Banks.Infrastructure.Configuration.Processing;

namespace App.Modules.Banks.Infrastructure;

public class BanksModule : IBanksModule
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
