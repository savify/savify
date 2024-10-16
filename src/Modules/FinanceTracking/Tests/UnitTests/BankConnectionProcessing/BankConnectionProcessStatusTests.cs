﻿using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;
using State = App.Modules.FinanceTracking.Domain.BankConnectionProcessing.BankConnectionProcessStatus.State;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing;

public class BankConnectionProcessStatusTests : UnitTestBase
{
    [Test]
    public void ToRedirected_HasValidTransitionFromInitiated()
    {
        var initiated = BankConnectionProcessStatusFactory.Create(State.Initiated);

        Assert.DoesNotThrow(() =>
        {
            initiated.ToRedirected();
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
            status.ToRedirected();
        });
    }

    [Test]
    public void ToRedirectUrlExpired_HasValidTransitionFromRedirected()
    {
        var redirected = BankConnectionProcessStatusFactory.Create(State.Redirected);

        Assert.DoesNotThrow(() =>
        {
            redirected.ToRedirectUrlExpired();
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
            status.ToRedirectUrlExpired();
        });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.Redirected)]
    public void ToErrorAtProvider_HasValidTransitionFrom(State initialState)
    {
        var status = BankConnectionProcessStatusFactory.Create(initialState);

        Assert.DoesNotThrow(() => { status.ToErrorAtProvider(); });
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

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() => { status.ToErrorAtProvider(); });
    }

    [Test]
    public void ToConsentRefused_HasValidTransitionFromRedirected()
    {
        var redirected = BankConnectionProcessStatusFactory.Create(State.Redirected);

        Assert.DoesNotThrow(() => { redirected.ToConsentRefused(); });
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

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() => { status.ToConsentRefused(); });
    }

    [Test]
    public void ToWaitingForAccountChoosing_HasValidTransitionFromRedirected()
    {
        var redirected = BankConnectionProcessStatusFactory.Create(State.Redirected);

        Assert.DoesNotThrow(() => { redirected.ToWaitingForAccountChoosing(); });
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
            status.ToWaitingForAccountChoosing();
        });
    }

    [Test]
    [TestCase(State.Redirected)]
    [TestCase(State.WaitingForAccountChoosing)]
    public void ToCompleted_HasValidTransitionFrom(State initialState)
    {
        var state = BankConnectionProcessStatusFactory.Create(initialState);

        Assert.DoesNotThrow(() => { state.ToCompleted(); });
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

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() => { status.ToCompleted(); });
    }

    [Test]
    [TestCase(State.Initiated)]
    [TestCase(State.Redirected)]
    [TestCase(State.WaitingForAccountChoosing)]
    public void ToCancelled_HasValidTransitionFrom(State initialState)
    {
        var state = BankConnectionProcessStatusFactory.Create(initialState);

        Assert.DoesNotThrow(() => { state.ToCancelled(); });
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

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() => { status.ToCancelled(); });
    }
}
