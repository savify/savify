namespace App.Modules.Transactions.Application.Contracts;

public interface ITransactionsModule
{
    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}
