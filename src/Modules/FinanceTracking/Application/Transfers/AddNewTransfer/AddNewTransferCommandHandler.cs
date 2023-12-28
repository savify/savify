using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class AddNewTransferCommandHandler(ITransfersRepository repository) : ICommandHandler<AddNewTransferCommand, Guid>
{
    public async Task<Guid> Handle(AddNewTransferCommand request, CancellationToken cancellationToken)
    {
        var transfer = Transfer.AddNew(
            new UserId(request.UserId),
            new WalletId(request.SourceWalletId),
            new WalletId(request.TargetWalletId),
            Money.From(request.Amount, request.Currency),
            request.MadeOn,
            request.Comment,
            request.Tags);

        await repository.AddAsync(transfer);

        return transfer.Id.Value;
    }
}
