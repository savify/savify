using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadata : Entity, IAggregateRoot
{
    public WalletId WalletId { get; private set; }

    public string? Color { get; private set; }

    public string? Icon { get; private set; }

    public bool IsConsideredInTotalBalance { get; private set; }

    public static WalletViewMetadata CreateForWallet(WalletId walletId, string? color, string? icon, bool isConsideredInTotalBalance)
    {
        return new WalletViewMetadata(walletId, color, icon, isConsideredInTotalBalance);
    }

    public static WalletViewMetadata CreateDefaultForWallet(WalletId walletId)
    {
        return CreateForWallet(walletId, color: null, icon: null, isConsideredInTotalBalance: true);
    }

    private WalletViewMetadata(WalletId walletId, string? color, string? icon, bool isConsideredInTotalBalance)
    {
        WalletId = walletId;
        Color = color;
        Icon = icon;
        IsConsideredInTotalBalance = isConsideredInTotalBalance;
    }

    private WalletViewMetadata() { }
}
