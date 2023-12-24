using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;

public class EditDebitWalletCommand(
    Guid userId,
    Guid walletId,
    string? title,
    string? currency,
    int? balance,
    string? color,
    string? icon,
    bool? considerInTotalBalance)
    : CommandBase
{
    public Guid UserId { get; } = userId;

    public Guid WalletId { get; } = walletId;

    public string? Title { get; } = title;

    public string? Currency { get; } = currency;

    public int? Balance { get; } = balance;

    public string? Color { get; } = color;

    public string? Icon { get; } = icon;

    public bool? ConsiderInTotalBalance { get; } = considerInTotalBalance;
}
