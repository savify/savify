using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.CreditWallets.EditCreditWallet;

public class EditCreditWalletCommand(
    Guid userId,
    Guid walletId,
    string? title,
    string? currency,
    int? availableBalance,
    int? creditLimit,
    string? color,
    string? icon,
    bool? considerInTotalBalance)
    : CommandBase
{
    public Guid UserId { get; } = userId;

    public Guid WalletId { get; } = walletId;

    public string? Title { get; } = title;

    public string? Currency { get; } = currency;

    public int? AvailableBalance { get; } = availableBalance;

    public int? CreditLimit { get; } = creditLimit;

    public string? Color { get; } = color;

    public string? Icon { get; } = icon;

    public bool? ConsiderInTotalBalance { get; } = considerInTotalBalance;
}
