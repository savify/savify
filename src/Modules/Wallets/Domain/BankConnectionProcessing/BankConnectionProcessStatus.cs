using App.BuildingBlocks.Domain;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;

namespace App.Modules.Wallets.Domain.BankConnectionProcessing;

public record BankConnectionProcessStatus(string Value)
{
    public static BankConnectionProcessStatus Initiated => new(nameof(Initiated));

    public BankConnectionProcessStatus ToRedirected() => TransitTo(Redirected);

    public BankConnectionProcessStatus ToRedirectUrlExpired() => TransitTo(RedirectUrlExpired);

    public BankConnectionProcessStatus ToErrorAtProvider() => TransitTo(ErrorAtProvider);

    public BankConnectionProcessStatus ToConsentRefused() => TransitTo(ConsentRefused);

    public BankConnectionProcessStatus ToWaitingForAccountChoosing() => TransitTo(WaitingForAccountChoosing);

    public BankConnectionProcessStatus ToCompleted() => TransitTo(Completed);

    public BankConnectionProcessStatus ToCancelled() => TransitTo(Cancelled);

    public bool IsFinal => FinalStatuses.Contains(this);

    public bool IsStatusTransitionValid(BankConnectionProcessStatus newStatus) => ValidStatusTransitions[this].Contains(newStatus);

    private BankConnectionProcessStatus TransitTo(BankConnectionProcessStatus newStatus)
    {
        var currentStatus = this;
        BusinessRuleChecker.CheckRules(new BankConnectionProcessStatusShouldKeepValidTransitionRule(currentStatus, newStatus));

        return newStatus;
    }

    private static readonly BankConnectionProcessStatus[] FinalStatuses = { RedirectUrlExpired, ErrorAtProvider, ConsentRefused, Completed, Cancelled };

    private static readonly IDictionary<BankConnectionProcessStatus, BankConnectionProcessStatus[]> ValidStatusTransitions = new Dictionary<BankConnectionProcessStatus, BankConnectionProcessStatus[]>
    {
        [Initiated] = new[] { Redirected, Cancelled },
        [Redirected] = new[] { RedirectUrlExpired, ErrorAtProvider, ConsentRefused, WaitingForAccountChoosing, Completed, Cancelled },
        [WaitingForAccountChoosing] = new[] { Completed, Cancelled }
    };

    private static BankConnectionProcessStatus Redirected => new(nameof(Redirected));

    private static BankConnectionProcessStatus RedirectUrlExpired => new(nameof(RedirectUrlExpired));

    private static BankConnectionProcessStatus ErrorAtProvider => new(nameof(ErrorAtProvider));

    private static BankConnectionProcessStatus ConsentRefused => new(nameof(ConsentRefused));

    private static BankConnectionProcessStatus WaitingForAccountChoosing => new(nameof(WaitingForAccountChoosing));

    private static BankConnectionProcessStatus Completed => new(nameof(Completed));

    private static BankConnectionProcessStatus Cancelled => new(nameof(Cancelled));
}
