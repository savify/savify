using App.Modules.Banks.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing;

internal static class QueryExecutor
{
    internal static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
    {
        using var scope = BanksCompositionRoot.BeginScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query);
    }
}
