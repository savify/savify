namespace App.Modules.Wallets.Domain.BankConnectionProcessing.Results;

public class Result<TSuccess, TError>
{
    public TSuccess Success { get; }

    public TError Error { get; }
}
