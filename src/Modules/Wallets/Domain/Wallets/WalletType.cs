namespace App.Modules.Wallets.Domain.Wallets;

public record WalletType(string Value)
{
    public static WalletType Cash = new WalletType(nameof(Cash));

    public static WalletType Debit = new WalletType(nameof(Debit));

    public static WalletType Credit = new WalletType(nameof(Credit));
}
