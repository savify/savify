namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public record BankConnectionProcessStatus(string Value)
{
    public static BankConnectionProcessStatus Initiated => new(nameof(Initiated));

    public static BankConnectionProcessStatus Redirected => new(nameof(Redirected));

    public static BankConnectionProcessStatus Expired => new(nameof(Expired));

    public static BankConnectionProcessStatus ErrorAtProvider => new(nameof(ErrorAtProvider));

    public static BankConnectionProcessStatus ConsentRefused => new(nameof(ConsentRefused));

    public static BankConnectionProcessStatus WaitingForAccountChoosing => new(nameof(WaitingForAccountChoosing));

    public static BankConnectionProcessStatus Completed => new(nameof(Completed));

    public bool IsFinal => FinalStatuses.Contains(this);

    private static readonly BankConnectionProcessStatus[] FinalStatuses = { Expired, ErrorAtProvider, ConsentRefused, Completed };
}
