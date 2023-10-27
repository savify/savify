using App.Modules.Categories.Application.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing;

internal static class QueryExecutor
{
    internal static async Task<TResult> Execute<TResult>(IQuery<TResult> query)
    {
        using var scope = CategoriesCompositionRoot.BeginScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(query);
    }
}
