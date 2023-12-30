using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.Application.Transfers.AddNewTransfer;

internal class AddNewTransferCommandHandler(ITransfersRepository repository) : ICommandHandler<AddNewTransferCommand, Guid>
{
    public async Task<Guid> Handle(AddNewTransferCommand command, CancellationToken cancellationToken)
    {
        var transfer = Transfer.AddNew(
            new UserId(command.UserId),
            new WalletId(command.SourceWalletId),
            new WalletId(command.TargetWalletId),
            Money.From(command.Amount, command.Currency),
            command.MadeOn,
            command.Comment,
            command.Tags);

        await repository.AddAsync(transfer);

        return transfer.Id.Value;
    }
}
