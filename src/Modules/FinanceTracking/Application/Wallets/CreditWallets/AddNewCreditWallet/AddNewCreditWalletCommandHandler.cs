using App.Modules.FinanceTracking.Application.Configuration.Commands;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;

internal class AddNewCreditWalletCommandHandler(CreditWalletFactory creditWalletFactory) : ICommandHandler<AddNewCreditWalletCommand, Guid>
{
    public async Task<Guid> Handle(AddNewCreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await creditWalletFactory.Create(
            new UserId(command.UserId),
            command.Title,
            Currency.From(command.Currency),
            command.CreditLimit,
            command.AvailableBalance,
            command.Color,
            command.Icon,
            command.ConsiderInTotalBalance);

        return wallet.Id.Value;
    }
}
