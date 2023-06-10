using App.BuildingBlocks.Infrastructure;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Notifications.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;
    private readonly NotificationsContext _notificationsContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated, 
        IUnitOfWork unitOfWork,
        NotificationsContext notificationsContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _notificationsContext = notificationsContext;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase<TResult>)
        {
            var internalCommand = await _notificationsContext.InternalCommands.FirstOrDefaultAsync(
                x => x.CausationId == command.Id && x.Type == command.GetType().FullName,
                cancellationToken: cancellationToken);
        
            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.UtcNow;
            }
        }
        
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return result;
    }
}
