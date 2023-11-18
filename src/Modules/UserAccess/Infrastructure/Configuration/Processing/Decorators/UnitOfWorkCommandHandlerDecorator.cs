using App.BuildingBlocks.Infrastructure;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using static App.Modules.UserAccess.Infrastructure.Configuration.Processing.Decorators.UnitOfWorkCommandHandlerDecorator;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IUnitOfWork<UserAccessContext> _unitOfWork;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        IUnitOfWork<UserAccessContext> unitOfWork)
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
    private readonly IUnitOfWork<UserAccessContext> _unitOfWork;
    private readonly UserAccessContext _userAccessContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T> decorated,
        IUnitOfWork<UserAccessContext> unitOfWork,
        UserAccessContext userAccessContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _userAccessContext = userAccessContext;
    }

    public async Task Handle(T command, CancellationToken cancellationToken)
    {
        await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase)
        {
            await SetInternalCommandAsProcessedAsync(_userAccessContext, command.Id, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}

internal static class UnitOfWorkCommandHandlerDecorator
{
    internal static async Task SetInternalCommandAsProcessedAsync(UserAccessContext userAccessContext, Guid commandId, CancellationToken cancellationToken)
    {
        var internalCommand = await userAccessContext.InternalCommands.SingleOrDefaultAsync(
            x => x.Id == commandId,
            cancellationToken: cancellationToken);

        if (internalCommand != null)
        {
            internalCommand.ProcessedDate = DateTime.UtcNow;
        }
    }
}
