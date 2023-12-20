using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.AddNewCreditWallet;
public class AddNewCreditWalletCommand(
    Guid userId,
    string title,
    string currency,
    int availableBalance,
    int creditLimit,
    string color,
    string icon,
    bool considerInTotalBalance)
    : CommandBase<Guid>
{
    public Guid UserId { get; } = userId;

    public string Title { get; } = title;

    public string Currency { get; } = currency;

    public int AvailableBalance { get; } = availableBalance;

    public int CreditLimit { get; } = creditLimit;

    public string Color { get; } = color;

    public string Icon { get; } = icon;

    public bool ConsiderInTotalBalance { get; } = considerInTotalBalance;
}
