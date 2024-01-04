using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;

internal class RemoveTransferCommandHandler(ITransferRepository repository) : ICommandHandler<RemoveTransferCommand>
{
    public async Task Handle(RemoveTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await repository.GetByIdAndUserIdAsync(new TransferId(command.TransferId), new UserId(command.UserId));

        transfer.Remove();

        repository.Remove(transfer);
    }
}
