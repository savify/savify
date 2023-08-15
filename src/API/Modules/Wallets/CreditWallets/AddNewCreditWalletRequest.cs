namespace App.API.Modules.Wallets.CreditWallets;

public class AddNewCreditWalletRequest
{
    public string Title { get; set; }

    public string Currency { get; set; }

    public int AvailableBalance { get; set; }

    public int CreditLimit { get; set; }
}