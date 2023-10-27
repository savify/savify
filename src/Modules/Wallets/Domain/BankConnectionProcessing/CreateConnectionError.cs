namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public record CreateConnectionError(string Type)
{
    public static CreateConnectionError ExternalProviderError = new(nameof(ExternalProviderError));
}
