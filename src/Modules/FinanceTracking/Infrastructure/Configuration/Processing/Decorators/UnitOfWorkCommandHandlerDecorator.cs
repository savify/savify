using App.BuildingBlocks.Infrastructure;
using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using static App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators.UnitOfWorkCommandHandlerDecorator;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IUnitOfWork<FinanceTrackingContext> _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        IUnitOfWork<FinanceTrackingContext> unitOfWork)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(command, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}

internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;
    private readonly IUnitOfWork<FinanceTrackingContext> _unitOfWork;
    private readonly FinanceTrackingContext _financeTrackingContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IUnitOfWork<FinanceTrackingContext> unitOfWork,
        FinanceTrackingContext financeTrackingContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _financeTrackingContext = financeTrackingContext;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            await SetInternalCommandAsProcessedAsync(_financeTrackingContext, command.Id, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
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
