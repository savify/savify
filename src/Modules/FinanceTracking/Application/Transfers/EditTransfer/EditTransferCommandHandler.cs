using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.EditTransfer;

internal class EditTransferCommandHandler(ITransfersRepository repository) : ICommandHandler<EditTransferCommand>
{
    public async Task Handle(EditTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = await repository.GetByIdAsync(new TransferId(request.TransferId));

        transfer.Edit(
            newSourceWalletId: new WalletId(request.SourceWalletId),
            newTargetWalletId: new WalletId(request.TargetWalletId),
            newAmount: Money.From(request.Amount, request.Currency),
            newMadeOn: request.MadeOn,
            newComment: request.Comment,
            newTags: request.Tags);
    }
}
