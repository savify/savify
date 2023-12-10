using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Application.Wallets.DebitWallets.EditDebitWallet;

public class EditDebitWalletCommand : CommandBase
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public string? Title { get; }

    public string? Currency { get; }

    public int? Balance { get; }

    public string? Color { get; }

    public string? Icon { get; }

    public bool? ConsiderInTotalBalance { get; }

    public EditDebitWalletCommand(Guid userId, Guid walletId, string? title, string? currency, int? balance, string? color, string? icon, bool? considerInTotalBalance)
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
