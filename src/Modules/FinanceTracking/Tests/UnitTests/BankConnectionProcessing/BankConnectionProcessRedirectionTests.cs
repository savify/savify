using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Users.FinanceTrackingSettings;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing;

[TestFixture]
public class BankConnectionProcessRedirectionTests : UnitTestBase
{
    private static UserId _userId = null!;

    private static BankId _bankId = null!;

    private static WalletId _walletId = null!;

    private static Language _userPreferredLanguage = null!;

    private static IBankConnectionProcessInitiationService _initiationService = null!;

    private static IBankConnectionProcessRedirectionService _redirectionService = null!;

    private static IBankConnectionProcessConnectionCreationService _connectionCreationService = null!;

    private static IBankAccountConnector _bankAccountConnector = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = new UserId(Guid.NewGuid());
        _bankId = new BankId(Guid.NewGuid());
        _walletId = new WalletId(Guid.NewGuid());
        _initiationService = Substitute.For<IBankConnectionProcessInitiationService>();
        _redirectionService = Substitute.For<IBankConnectionProcessRedirectionService>();
        _connectionCreationService = Substitute.For<IBankConnectionProcessConnectionCreationService>();
        _bankAccountConnector = Substitute.For<IBankAccountConnector>();
    }

    [Test]
    public async Task RedirectingUser_InBankConnectionProcess_IsSuccessful()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId, _userPreferredLanguage).Returns(redirection);

        var redirectionResult = await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);

        var userRedirectedDomainEvent = AssertPublishedDomainEvent<UserRedirectedDomainEvent>(bankConnectionProcess);
        Assert.That(redirectionResult.Success, Is.EqualTo(redirection.Url));
        Assert.That(userRedirectedDomainEvent.BankConnectionProcessId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(userRedirectedDomainEvent.ExpiresAt, Is.EqualTo(redirection.ExpiresAt));
    }

    [Test]
    public async Task RedirectingUser_WhenExternalProviderErrorOccures_ReturnsExternalProviderErrorResult()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId, _userPreferredLanguage).Returns(RedirectionError.ExternalProviderError);

        var redirectionResult = await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);

        Assert.That(redirectionResult.IsError, Is.True);

        var error = redirectionResult.Error;
        Assert.That(error, Is.EqualTo(RedirectionError.ExternalProviderError));
    }

    [Test]
    public async Task RedirectingUser_WhenRedirectUrlIsExpired_BreaksBankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MinValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId, _userPreferredLanguage).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);
        bankConnectionProcess.Expire();

        AssertBrokenRuleAsync<BankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule>(async Task () =>
        {
            await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);
        });
    }

    [Test]
    public async Task RedirectingUser_WhenBankConnectionProcessIsInFinalStatus_BreaksCannotOperateOnBankConnectionProcessWithFinalStatusRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId, _userPreferredLanguage).Returns(redirection);

        var bankConnectionStub = BankConnection.CreateFromBankConnectionProcess(bankConnectionProcess.Id, _bankId, _userId, new Consent(DateTime.MaxValue));
        bankConnectionStub.AddBankAccount("123", "Test Account 1", 100, Currency.From("USD"));

        _connectionCreationService
            .CreateConnection(bankConnectionProcess.Id, _userId, _bankId, "123456")
            .Returns(bankConnectionStub);

        await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);
        await bankConnectionProcess.CreateConnection("123456", _connectionCreationService, _bankAccountConnector);

        AssertBrokenRuleAsync<CannotOperateOnBankConnectionProcessWithFinalStatusRule>(async Task () =>
        {
            await bankConnectionProcess.Redirect(_redirectionService, _userPreferredLanguage);
        });
    }
}
