namespace App.Modules.FinanceTracking.Application.BankConnectionProcessing.CreateBankConnection;

public record CreateBankConnectionError(string Value)
{
    public static CreateBankConnectionError ExternalProviderError => new(nameof(ExternalProviderError));
}
