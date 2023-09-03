namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public record BankConnectionProcessStatus(string Value)
{
    public static BankConnectionProcessStatus Initiated => new(nameof(Initiated));

    public static BankConnectionProcessStatus Redirected => new(nameof(Redirected));

    public static BankConnectionProcessStatus RedirectUrlExpired => new(nameof(RedirectUrlExpired));

    public static BankConnectionProcessStatus ErrorAtProvider => new(nameof(ErrorAtProvider));

    public static BankConnectionProcessStatus ConsentRefused => new(nameof(ConsentRefused));

    public static BankConnectionProcessStatus WaitingForAccountChoosing => new(nameof(WaitingForAccountChoosing));

    public static BankConnectionProcessStatus Completed => new(nameof(Completed));

    public static BankConnectionProcessStatus Cancelled => new(nameof(Cancelled));

    public bool IsFinal => FinalStatuses.Contains(this);

    public bool IsStatusTransitionValid(BankConnectionProcessStatus newStatus) => ValidStatusTransitions[this].Contains(newStatus);

    private static readonly BankConnectionProcessStatus[] FinalStatuses = { RedirectUrlExpired, ErrorAtProvider, ConsentRefused, Completed, Cancelled };

    private static readonly IDictionary<BankConnectionProcessStatus, BankConnectionProcessStatus[]> ValidStatusTransitions = new Dictionary<BankConnectionProcessStatus, BankConnectionProcessStatus[]>
        {
            {Initiated, new []{Redirected, Cancelled}},
            {Redirected, new []{RedirectUrlExpired, ErrorAtProvider, ConsentRefused, WaitingForAccountChoosing, Completed, Cancelled}},
            {WaitingForAccountChoosing, new []{Completed, Cancelled}}
        };
}
