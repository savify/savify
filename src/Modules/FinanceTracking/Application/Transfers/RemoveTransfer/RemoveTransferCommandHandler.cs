using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Transfers;

namespace App.Modules.FinanceTracking.Application.Transfers.RemoveTransfer;

internal class RemoveTransferCommandHandler(ITransferRepository repository) : ICommandHandler<RemoveTransferCommand>
{
    public async Task Handle(RemoveTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = await repository.GetByIdAsync(new TransferId(command.TransferId));

        transfer.Remove();

        repository.Remove(transfer);
    }
}
