namespace App.API.Modules.Wallets.CashWallets.Requests;

public class EditCashWalletRequest
{
    public Guid WalletId { get; set; }

    public string? Title { get; set; }

    public string? Currency { get; set; }

    public int? Balance { get; set; }

    public string? Color { get; set; }

    public string? Icon { get; set; }

    public bool? ConsiderInTotalBalance { get; set; }
}
