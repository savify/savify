namespace App.Modules.FinanceTracking.Application.Contracts;

public interface IFinanceTrackingModule
{
    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}
