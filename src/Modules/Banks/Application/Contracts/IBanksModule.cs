namespace App.Modules.Banks.Application.Contracts;

public interface IBanksModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}
