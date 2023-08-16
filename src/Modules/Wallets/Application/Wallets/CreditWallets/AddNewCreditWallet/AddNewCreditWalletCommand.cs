using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.AddNewCreditWallet;
public class AddNewCreditWalletCommand : CommandBase<Guid>
{
    public Guid UserId { get; }

    public string Title { get; }

    public string Currency { get; }

    public int AvailableBalance { get; }

    public int CreditLimit { get; }

    public string Color { get; }

    public string Icon { get; }

    public bool ConsiderInTotalBalance { get; }

    public AddNewCreditWalletCommand(Guid userId, string title, string currency, int availableBalance, int creditLimit, string color, string icon, bool considerInTotalBalance)
    {
        UserId = userId;
        Title = title;
        Currency = currency;
        AvailableBalance = availableBalance;
        CreditLimit = creditLimit;
        Color = color;
        Icon = icon;
        ConsiderInTotalBalance = considerInTotalBalance;
    }
}
