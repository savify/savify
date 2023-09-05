namespace App.Modules.Wallets.Application.Wallets;

public class BankConnectionProcessInitiationResult
{
    public Guid Id { get; }

    public string RedirectUrl { get; }

    public BankConnectionProcessInitiationResult(Guid id, string redirectUrl)
    {
        Id = id;
        RedirectUrl = redirectUrl;
    }
}
