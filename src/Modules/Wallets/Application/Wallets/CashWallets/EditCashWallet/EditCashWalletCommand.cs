using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CashWallets.EditCashWallet;

public class EditCashWalletCommand : CommandBase<Result>
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public string? Title { get; }

    public string? Currency { get; }

    public int? Balance { get; }

    public string? Color { get; }

    public string? Icon { get; }

    public bool? ConsiderInTotalBalance { get; }

    public EditCashWalletCommand(Guid userId, Guid walletId, string? title, string? currency, int? balance, string? color, string? icon, bool? considerInTotalBalance)
    {
        UserId = userId;
        WalletId = walletId;
        Title = title;
        Currency = currency;
        Balance = balance;
        Color = color;
        Icon = icon;
        ConsiderInTotalBalance = considerInTotalBalance;
    }
}
