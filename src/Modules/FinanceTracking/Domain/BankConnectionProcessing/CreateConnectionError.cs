namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing;

public record CreateConnectionError(string Type)
{
    public static CreateConnectionError ExternalProviderError = new(nameof(ExternalProviderError));
}
