using App.BuildingBlocks.Infrastructure;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using static App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators.UnitOfWorkCommandHandlerDecorator;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork<FinanceTrackingContext> unitOfWork)
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
    IUnitOfWork<FinanceTrackingContext> unitOfWork,
    FinanceTrackingContext financeTrackingContext)
    : ICommandHandler<T>
    where T : ICommand
{
    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            await SetInternalCommandAsProcessedAsync(financeTrackingContext, command.Id, cancellationToken);
        }

        await unitOfWork.CommitAsync(cancellationToken);
    }
}

internal static class UnitOfWorkCommandHandlerDecorator
{
    internal static async Task SetInternalCommandAsProcessedAsync(FinanceTrackingContext financeTrackingContext, Guid commandId, CancellationToken cancellationToken)
    {
        var internalCommand = await financeTrackingContext.InternalCommands.SingleOrDefaultAsync(
            x => x.Id == commandId,
            cancellationToken: cancellationToken);

        if (internalCommand != null)
        {
            internalCommand.ProcessedDate = DateTime.UtcNow;
        }
    }
}
