using App.Modules.Transactions.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Transactions.Infrastructure.Configuration.Processing;

internal static class CommandExecutor
{
    internal static async Task Execute(ICommand command)
    {
        using var scope = TransactionsCompositionRoot.BeginScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(command);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        using var scope = TransactionsCompositionRoot.BeginScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(command);
    }
}
