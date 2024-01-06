namespace App.API.Modules.FinanceTracking.Wallets.DebitWallets.Requests;

public class EditDebitWalletRequest
{
    public string? Title { get; set; }

    public int? Balance { get; set; }

    public string? Color { get; set; }

    public string? Icon { get; set; }

    public bool? ConsiderInTotalBalance { get; set; }
}
