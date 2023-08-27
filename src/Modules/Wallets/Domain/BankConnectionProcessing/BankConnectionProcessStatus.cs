namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public record BankConnectionProcessStatus(string Value, bool IsFinal)
{
    public static BankConnectionProcessStatus Initiated => new(nameof(Initiated), false);

    public static BankConnectionProcessStatus Redirected => new(nameof(Redirected), false);

    public static BankConnectionProcessStatus Expired => new(nameof(Expired), true);

    public static BankConnectionProcessStatus ErrorAtProvider => new(nameof(ErrorAtProvider), true);

    public static BankConnectionProcessStatus ConsentConfirmed => new(nameof(ConsentConfirmed), false);

    public static BankConnectionProcessStatus ConsentRefused => new(nameof(ConsentRefused), true);

    public static BankConnectionProcessStatus WaitingForAccountChoosing => new(nameof(WaitingForAccountChoosing), false);

    public static BankConnectionProcessStatus Completed => new(nameof(Completed), true);


}
