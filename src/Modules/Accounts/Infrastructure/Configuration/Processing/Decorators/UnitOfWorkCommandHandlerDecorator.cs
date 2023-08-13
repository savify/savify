using App.BuildingBlocks.Infrastructure;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Accounts.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AccountsContext _accountsContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated, 
        IUnitOfWork unitOfWork,
        AccountsContext accountsContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _accountsContext = accountsContext;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase<TResult>)
        {
            var internalCommand = await _accountsContext.InternalCommands.FirstOrDefaultAsync(
                x => x.Id == command.Id, cancellationToken: cancellationToken);
        
            if (internalCommand != null)
            {
                internalCommand.ProcessedDate = DateTime.UtcNow;
            }
        }
        
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return result;
    }
}
