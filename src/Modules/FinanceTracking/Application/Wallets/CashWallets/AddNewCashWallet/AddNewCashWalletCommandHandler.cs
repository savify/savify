using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using Currency = App.Modules.FinanceTracking.Domain.Finance.Currency;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;

internal class AddNewCashWalletCommandHandler(CashWalletFactory cashWalletFactory) : ICommandHandler<AddNewCashWalletCommand, Guid>
{
    public async Task<Guid> Handle(AddNewCashWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await cashWalletFactory.Create(
            new UserId(command.UserId),
            command.Title,
            new Currency(command.Currency),
            command.Balance,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        return wallet.Id.Value;
    }
}
