namespace App.Modules.FinanceTracking.Application.Wallets;

public class BankConnectionProcessInitiationSuccess
{
    public Guid Id { get; }

    public string RedirectUrl { get; }

    public BankConnectionProcessInitiationSuccess(Guid id, string redirectUrl)
    {
        Id = id;
        RedirectUrl = redirectUrl;
    }
}
