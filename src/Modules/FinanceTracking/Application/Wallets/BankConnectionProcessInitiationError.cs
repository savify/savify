namespace App.Modules.FinanceTracking.Application.Wallets;

public record BankConnectionProcessInitiationError(string Type)
{
    public static BankConnectionProcessInitiationError ExternalProviderError => new(nameof(ExternalProviderError));
}
