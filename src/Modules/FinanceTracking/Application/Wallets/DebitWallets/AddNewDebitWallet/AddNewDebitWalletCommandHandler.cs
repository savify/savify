using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.AddNewDebitWallet;

internal class AddNewDebitWalletCommandHandler(DebitWalletFactory debitWalletFactory) : ICommandHandler<AddNewDebitWalletCommand, Guid>
{
    public async Task<Guid> Handle(AddNewDebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await debitWalletFactory.Create(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.Balance,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        return wallet.Id.Value;
    }
}
