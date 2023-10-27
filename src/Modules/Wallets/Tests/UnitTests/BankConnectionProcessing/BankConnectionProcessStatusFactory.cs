using App.Modules.Wallets.Domain.BankConnectionProcessing;
using State = App.Modules.Wallets.Domain.BankConnectionProcessing.BankConnectionProcessStatus.State;

namespace App.Modules.Wallets.UnitTests.BankConnectionProcessing;

public class BankConnectionProcessStatusFactory
{
    public static BankConnectionProcessStatus Create(State state) => state switch
    {
        State.Initiated => BankConnectionProcessStatus.Initiated,
        State.Redirected => BankConnectionProcessStatus.Initiated.ToRedirected(),
        State.RedirectUrlExpired => BankConnectionProcessStatus.Initiated.ToRedirected().ToRedirectUrlExpired(),
        State.ErrorAtProvider => BankConnectionProcessStatus.Initiated.ToRedirected().ToErrorAtProvider(),
        State.ConsentRefused => BankConnectionProcessStatus.Initiated.ToRedirected().ToConsentRefused(),
        State.WaitingForAccountChoosing => BankConnectionProcessStatus.Initiated.ToRedirected().ToWaitingForAccountChoosing(),
        State.Completed => BankConnectionProcessStatus.Initiated.ToRedirected().ToCompleted(),
        State.Cancelled => BankConnectionProcessStatus.Initiated.ToCancelled(),
        _ => throw new NotImplementedException($"Unknow bank connection process status: {state}"),
    };
}
