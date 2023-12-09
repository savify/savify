namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing;

public record RedirectionError(string Type)
{
    public static RedirectionError ExternalProviderError = new(nameof(ExternalProviderError));
}
