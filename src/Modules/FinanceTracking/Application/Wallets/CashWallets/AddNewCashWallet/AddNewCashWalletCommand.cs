using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CashWallets.AddNewCashWallet;

public class AddNewCashWalletCommand(
    Guid userId,
    string title,
    string currency,
    int initialBalance,
    string color,
    string icon,
    bool considerInTotalBalance)
    : CommandBase<Guid>
{
    public Guid UserId { get; } = userId;

    public string Title { get; } = title;

    public string Currency { get; } = currency;

    public int InitialBalance { get; } = initialBalance;

    public string Color { get; } = color;

    public string Icon { get; } = icon;

    public bool ConsiderInTotalBalance { get; } = considerInTotalBalance;
}
