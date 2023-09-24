using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.CreditWallets.EditCreditWallet;

public class EditCreditWalletCommand : CommandBase
{
    public Guid UserId { get; }

    public Guid WalletId { get; }

    public string? Title { get; }

    public string? Currency { get; }

    public int? AvailableBalance { get; }

    public int? CreditLimit { get; }

    public string? Color { get; }

    public string? Icon { get; }

    public bool? ConsiderInTotalBalance { get; }

    public EditCreditWalletCommand(Guid userId, Guid walletId, string? title, string? currency, int? availableBalance, int? creditLimit, string? color, string? icon, bool? considerInTotalBalance)
    {
        UserId = userId;
        WalletId = walletId;
        Title = title;
        Currency = currency;
        AvailableBalance = availableBalance;
        CreditLimit = creditLimit;
        Color = color;
        Icon = icon;
        ConsiderInTotalBalance = considerInTotalBalance;
    }
}
