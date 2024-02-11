using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing;

[TestFixture]
public class BankConnectionProcessExpirationTests : UnitTestBase
{
    private static UserId _userId = null!;

    private static BankId _bankId = null!;

    private static WalletId _walletId = null!;

    private static Language _userPreferredLanguage = null!;

    private static IBankConnectionProcessInitiationService _initiationService = null!;

    private static IBankConnectionProcessRedirectionService _redirectionService = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = new UserId(Guid.NewGuid());
        _bankId = new BankId(Guid.NewGuid());
        _walletId = new WalletId(Guid.NewGuid());
        _initiationService = Substitute.For<IBankConnectionProcessInitiationService>();
        _redirectionService = Substitute.For<IBankConnectionProcessRedirectionService>();
    }


    [Test]
    public async Task ExpiringBankConnectionProcess_IsSuccessful()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MinValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId, _userPreferredLanguage).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);

        bankConnectionProcess.Expire();

        var expiredDomainEvent = AssertPublishedDomainEvent<BankConnectionProcessExpiredDomainEvent>(bankConnectionProcess);
        Assert.That(expiredDomainEvent.BankConnectionProcessId, Is.EqualTo(bankConnectionProcess.Id));
    }

    [Test]
    public async Task ExpiringBankConnectionProcess_WhenRedirectUrlIsNotExpired_BreaksRedirectUrlShouldBeExpiredRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId, _userPreferredLanguage).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);

        AssertBrokenRule<RedirectUrlShouldBeExpiredRule>(() =>
        {
            bankConnectionProcess.Expire();
        });
    }

    [Test]
    public async Task ExpiringBankConnectionProcess_WhenNotInRedirectedStatus_BreaksBankConnectionProcessShouldKeepValidStatusTransitionsRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        AssertBrokenRule<BankConnectionProcessStatusShouldKeepValidTransitionRule>(() =>
        {
            bankConnectionProcess.Expire();
        });
    }
}
