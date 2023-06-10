namespace App.Modules.Accounts.Application.Contracts;

public interface IAccountsModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
    
    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}
