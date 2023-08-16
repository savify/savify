namespace App.API.Modules.Wallets.CashWallets;

public class AddNewCashWalletRequest
{
    public string Title { get; set; }

    public string Currency { get; set; }

    public int Balance { get; set; }
}
