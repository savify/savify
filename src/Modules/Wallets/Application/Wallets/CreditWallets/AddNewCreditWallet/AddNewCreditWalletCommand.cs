using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.AddNewCreditWallet;
public class AddNewCreditWalletCommand : CommandBase<Guid>
{
    public Guid UserId { get; }

    public string Title { get; }

    public string Currency { get; }

    public int AvailableBalance { get; }

    public int CreditLimit { get; }

    public AddNewCreditWalletCommand(Guid userId, string title, string currency, int availableBalance, int creditLimit)
    {
        UserId = userId;
        Title = title;
        AvailableBalance = availableBalance;
        CreditLimit = creditLimit;
        Currency = currency;
    }
}
