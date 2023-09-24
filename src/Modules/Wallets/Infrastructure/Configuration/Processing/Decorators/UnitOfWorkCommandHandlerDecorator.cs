using App.BuildingBlocks.Infrastructure;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using static App.Modules.Wallets.Infrastructure.Configuration.Processing.Decorators.UnitOfWorkCommandHandlerDecorator;

namespace App.Modules.Wallets.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;
    private readonly WalletsContext _walletsContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        IUnitOfWork unitOfWork,
        WalletsContext walletsContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _walletsContext = walletsContext;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase<TResult>)
        {
            await SetInternalCommandAsProcessedAsync(_walletsContext, command.Id, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }
}

internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ICommandHandler<T> _decorated;
    private readonly IUnitOfWork _unitOfWork;
    private readonly WalletsContext _walletsContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IUnitOfWork unitOfWork,
        WalletsContext walletsContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _walletsContext = walletsContext;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            await SetInternalCommandAsProcessedAsync(_walletsContext, command.Id, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

internal static class UnitOfWorkCommandHandlerDecorator
{
    internal static async Task SetInternalCommandAsProcessedAsync(WalletsContext walletsContext, Guid commandId, CancellationToken cancellationToken)
    {
        var internalCommand = await walletsContext.InternalCommands.SingleOrDefaultAsync(
            x => x.Id == commandId,
            cancellationToken: cancellationToken);

        if (internalCommand != null)
        {
            internalCommand.ProcessedDate = DateTime.UtcNow;
        }
    }
}
