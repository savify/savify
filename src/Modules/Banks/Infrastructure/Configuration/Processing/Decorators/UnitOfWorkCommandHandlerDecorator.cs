using App.BuildingBlocks.Infrastructure;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Banks.Infrastructure.Configuration.Processing.Decorators;

internal class UnitOfWorkCommandHandlerDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
{
    private readonly ICommandHandler<T, TResult> _decorated;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BanksContext _banksContext;

    public UnitOfWorkCommandHandlerDecorator(
        ICommandHandler<T, TResult> decorated,
        IUnitOfWork unitOfWork,
        BanksContext banksContext)
    {
        _decorated = decorated;
        _unitOfWork = unitOfWork;
        _banksContext = banksContext;
    }

    public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
    {
        var result = await _decorated.Handle(command, cancellationToken);

        if (command is InternalCommandBase<TResult>)
        {
            var internalCommand = await _banksContext.InternalCommands.SingleOrDefaultAsync(
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
