using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

public class WalletViewMetadata : Entity, IAggregateRoot
{
    public WalletId WalletId { get; private set; }

    public string Color { get; private set; }

    public string Icon { get; private set; }

    public bool IsConsideredInTotalBalance { get; private set; }

    public static WalletViewMetadata CreateForWallet(WalletId walletId, string color, string icon, bool isConsideredInTotalBalance)
    {
        return new WalletViewMetadata(walletId, color, icon, isConsideredInTotalBalance);
    }

    public void Edit(string? newColor, string? newIcon, bool? isConsideredInTotalBalance)
    {
        Color = newColor ?? Color;
        Icon = newIcon ?? Icon;
        IsConsideredInTotalBalance = isConsideredInTotalBalance ?? IsConsideredInTotalBalance;
    }

    private WalletViewMetadata(WalletId walletId, string color, string icon, bool isConsideredInTotalBalance)
    {
        WalletId = walletId;
        Color = color;
        Icon = icon;
        IsConsideredInTotalBalance = isConsideredInTotalBalance;
    }

    private WalletViewMetadata() { }
}
