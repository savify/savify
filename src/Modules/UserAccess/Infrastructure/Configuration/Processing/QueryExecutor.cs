using App.BuildingBlocks.Infrastructure.Configuration;
using App.Modules.UserAccess.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing;

internal static class QueryExecutor
{
    internal static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
    {
        using var scope = CompositionRoot.BeginScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query);
    }
}
