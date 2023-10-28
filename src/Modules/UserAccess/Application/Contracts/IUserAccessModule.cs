namespace App.Modules.UserAccess.Application.Contracts;

public interface IUserAccessModule
{
    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}
