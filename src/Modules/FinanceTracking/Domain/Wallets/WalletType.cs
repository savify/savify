namespace App.Modules.FinanceTracking.Domain.Wallets;

public record WalletType(string Value)
{
    public static WalletType Cash = new(nameof(Cash));

    public static WalletType Debit = new(nameof(Debit));

    public static WalletType Credit = new(nameof(Credit));
}
