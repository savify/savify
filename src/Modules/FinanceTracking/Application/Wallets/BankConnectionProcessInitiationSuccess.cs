namespace App.Modules.FinanceTracking.Application.Wallets;

public class BankConnectionProcessInitiationSuccess(Guid id, string redirectUrl)
{
    public Guid Id { get; } = id;

    public string RedirectUrl { get; } = redirectUrl;
}
