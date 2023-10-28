using App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;
using State = App.Modules.Wallets.Domain.BankConnectionProcessing.BankConnectionProcessStatus.State;

namespace App.Modules.Wallets.UnitTests.BankConnectionProcessing;

public class BankConnectionProcessStatusTests : UnitTestBase
{
    [Test]
    public void ToRedirected_HasValidTransitionFromInitiated()
    {
        var initiated = BankConnectionProcessStatusFactory.Create(State.Initiated);

        Assert.DoesNotThrow(() =>
        {
            var redirected = initiated.ToRedirected();
        });
    }

    [Test]
    [TestCase(State.Redirected)]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.WaitingForAccountChoosing)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToRedirected_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var redirected = status.ToRedirected();
        });
    }

    [Test]
    public void ToRedirectUrlExpired_HasValidTransitionFromRedirected()
    {
        var redirected = BankConnectionProcessStatusFactory.Create(State.Redirected);

        Assert.DoesNotThrow(() =>
        {
            var redirectUrlExpired = redirected.ToRedirectUrlExpired();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.WaitingForAccountChoosing)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToRedirectUrlExpired_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var redirectUrlExpired = status.ToRedirectUrlExpired();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.Redirected)]
    public void ToErrorAtProvider_HasValidTransitionFrom(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        Assert.DoesNotThrow(() =>
        {
            var errorAtProvider = status.ToErrorAtProvider();
        });
    }

    [Test]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.WaitingForAccountChoosing)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToErrorAtProvider_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var errorAtProvider = status.ToErrorAtProvider();
        });
    }

    [Test]
    public void ToConsentRefused_HasValidTransitionFromRedirected()
    {
        var redirected = BankConnectionProcessStatusFactory.Create(State.Redirected);

        Assert.DoesNotThrow(() =>
        {
            var consentRefused = redirected.ToConsentRefused();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.WaitingForAccountChoosing)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToConsentRefused_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var errorAtProvider = status.ToConsentRefused();
        });
    }

    [Test]
    public void ToWaitingForAccountChoosing_HasValidTransitionFromRedirected()
    {
        var redirected = BankConnectionProcessStatusFactory.Create(State.Redirected);

        Assert.DoesNotThrow(() =>
        {
            var waitingForAccountChoosing = redirected.ToWaitingForAccountChoosing();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.WaitingForAccountChoosing)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToWaitingForAccountChoosing_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var waitingForAccountChoosing = status.ToWaitingForAccountChoosing();
        });
    }

    [Test]
    [TestCase(State.Redirected)]
    [TestCase(State.WaitingForAccountChoosing)]
    public void ToCompleted_HasValidTransitionFrom(State initialState)
    {
        var state = BankConnectionProcessStatusFactory.Create(initialState);

        Assert.DoesNotThrow(() =>
        {
            var completed = state.ToCompleted();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToCompleted_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var completed = status.ToCompleted();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.Redirected)]
    [TestCase(State.WaitingForAccountChoosing)]
    public void ToCancelled_HasValidTransitionFrom(State initialState)
    {
        var state = BankConnectionProcessStatusFactory.Create(initialState);

        Assert.DoesNotThrow(() =>
        {
            var cancelled = state.ToCancelled();
        });
    }

    [Test]
    [TestCase(State.RedirectUrlExpired)]
    [TestCase(State.ErrorAtProvider)]
    [TestCase(State.ConsentRefused)]
    [TestCase(State.Completed)]
    [TestCase(State.Cancelled)]
    public void ToCanceled_DoesNotHaveValidTransitionFromOtherStatuses(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            var canceled = status.ToCancelled();
        });
    }
}
