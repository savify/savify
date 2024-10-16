namespace App.API.Modules.FinanceTracking.Wallets.DebitWallets.Requests;

public class AddNewDebitWalletRequest
{
    public string Title { get; set; }

    public string Currency { get; set; }

    public int Balance { get; set; }

    public string Color { get; set; }

    public string Icon { get; set; }

    public bool ConsiderInTotalBalance { get; set; }
}
