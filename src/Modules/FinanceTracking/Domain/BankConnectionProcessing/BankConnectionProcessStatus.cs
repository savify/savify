using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;

namespace App.Modules.FinanceTracking.Domain.BankConnectionProcessing;

public record BankConnectionProcessStatus
{
    private readonly State _value;

    public State Value => _value;

    private BankConnectionProcessStatus(State value)
    {
        _value = value;
    }

    public static BankConnectionProcessStatus Initiated => new(State.Initiated);

    public BankConnectionProcessStatus ToRedirected() => TransitTo(State.Redirected);

    public BankConnectionProcessStatus ToRedirectUrlExpired() => TransitTo(State.RedirectUrlExpired);

    public BankConnectionProcessStatus ToErrorAtProvider() => TransitTo(State.ErrorAtProvider);

    public BankConnectionProcessStatus ToConsentRefused() => TransitTo(State.ConsentRefused);

    public BankConnectionProcessStatus ToWaitingForAccountChoosing() => TransitTo(State.WaitingForAccountChoosing);

    public BankConnectionProcessStatus ToCompleted() => TransitTo(State.Completed);

    public BankConnectionProcessStatus ToCancelled() => TransitTo(State.Cancelled);

    public bool IsFinal => FinalStatuses.Contains(Value);

    public bool IsStatusTransitionValid(BankConnectionProcessStatus newStatus)
    {
        var currentStatus = this;

        var canCurrentStatusBeTransited = ValidStatusTransitions.TryGetValue(currentStatus.Value, out var availableTransitionStates);
        if (canCurrentStatusBeTransited)
        {
            return availableTransitionStates!.Contains(newStatus.Value);
        }

        return false;
    }

    private BankConnectionProcessStatus TransitTo(State state)
    {
        var currentStatus = this;
        var newStatus = new BankConnectionProcessStatus(state);

        BusinessRuleChecker.CheckRules(new BankConnectionProcessStatusShouldKeepValidTransitionRule(currentStatus, newStatus));

        return newStatus;
    }

    private static readonly State[] FinalStatuses = { State.RedirectUrlExpired, State.ErrorAtProvider, State.ConsentRefused, State.Completed, State.Cancelled };

    private static readonly IDictionary<State, State[]> ValidStatusTransitions = new Dictionary<State, State[]>
    {
        [State.Initiated] = new[] { State.Redirected, State.Cancelled, State.ErrorAtProvider },
        [State.Redirected] = new[] { State.RedirectUrlExpired, State.ErrorAtProvider, State.ConsentRefused, State.WaitingForAccountChoosing, State.Completed, State.Cancelled },
        [State.WaitingForAccountChoosing] = new[] { State.Completed, State.Cancelled }
    };

    public enum State
    {
        Initiated,
        Redirected,
        RedirectUrlExpired,
        ErrorAtProvider,
        ConsentRefused,
        WaitingForAccountChoosing,
        Completed,
        Cancelled
    }
}
