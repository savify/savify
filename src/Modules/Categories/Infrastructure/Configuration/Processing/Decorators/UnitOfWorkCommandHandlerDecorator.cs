using App.BuildingBlocks.Infrastructure;
using App.Modules.Categories.Application.Configuration.Commands;
using App.Modules.Categories.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using static App.Modules.Categories.Infrastructure.Configuration.Processing.Decorators.UnitOfWorkCommandHandlerDecorator;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork<CategoriesContext> unitOfWork)
    : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(command, cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}

internal class UnitOfWorkCommandHandlerDecorator<T>(
    ICommandHandler<T> decorated,
    IUnitOfWork<CategoriesContext> unitOfWork,
    CategoriesContext categoriesContext)
    : ICommandHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            await SetInternalCommandAsProcessedAsync(categoriesContext, command.Id, cancellationToken);
        }

        await unitOfWork.CommitAsync(cancellationToken);
    }
}

internal static class UnitOfWorkCommandHandlerDecorator
{
    internal static async Task SetInternalCommandAsProcessedAsync(CategoriesContext categoriesContext, Guid commandId, CancellationToken cancellationToken)
    {
        var internalCommand = await categoriesContext.InternalCommands.SingleOrDefaultAsync(
            x => x.Id == commandId,
            cancellationToken: cancellationToken);

        if (internalCommand != null)
        {
            internalCommand.ProcessedDate = DateTime.UtcNow;
        }
    }
}
