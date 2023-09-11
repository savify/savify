using App.Modules.Wallets.Domain.BankConnectionProcessing;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Events;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Rules;
using App.Modules.Wallets.Domain.BankConnectionProcessing.Services;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.Finance;
using App.Modules.Wallets.Domain.Users;
using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.BankAccountConnections;

namespace App.Modules.Wallets.UnitTests.BankConnectionProcessing;

[TestFixture]
public class BankConnectionProcessRedirectionTests : UnitTestBase
{
    private static UserId _userId;

    private static BankId _bankId;

    private static WalletId _walletId;

    private static IBankConnectionProcessInitiationService _initiationService;

    private static IBankConnectionProcessRedirectionService _redirectionService;

    private static IBankConnectionProcessConnectionCreationService _connectionCreationService;

    private static IBankAccountConnector _bankAccountConnector;

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
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId).Returns(redirection);

        var redirectUrl = await bankConnectionProcess.Redirect(_redirectionService);

        var userRedirectedDomainEvent = AssertPublishedDomainEvent<UserRedirectedDomainEvent>(bankConnectionProcess);
        Assert.That(redirectUrl, Is.EqualTo(redirection.Url));
        Assert.That(userRedirectedDomainEvent.BankConnectionProcessId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(userRedirectedDomainEvent.ExpiresAt, Is.EqualTo(redirection.ExpiresAt));
    }

    [Test]
    public async Task RedirectingUser_WhenRedirectUrlIsExpired_BreaksBankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MinValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService);
        bankConnectionProcess.Expire();

        AssertBrokenRuleAsync<BankConnectionProcessCannotMakeRedirectionWhenRedirectUrlIsExpiredRule>(async Task () =>
        {
            await bankConnectionProcess.Redirect(_redirectionService);
        });
    }

    [Test]
    public async Task RedirectingUser_WhenBankConnectionProcessIsInFinalStatus_BreaksCannotOperateOnBankConnectionProcessWithFinalStatusRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId).Returns(redirection);

        var bankConnectionStub = BankConnection.CreateFromBankConnectionProcess(bankConnectionProcess.Id, _bankId, _userId, new Consent(DateTime.MaxValue));
        bankConnectionStub.AddBankAccount("123", "Test Account 1", 100, new Currency("USD"));

        _connectionCreationService
            .CreateConnection(bankConnectionProcess.Id, _userId, _bankId, "123456")
            .Returns(bankConnectionStub);

        await bankConnectionProcess.Redirect(_redirectionService);
        await bankConnectionProcess.CreateConnection("123456", _connectionCreationService, _bankAccountConnector);

        AssertBrokenRuleAsync<CannotOperateOnBankConnectionProcessWithFinalStatusRule>(async Task () =>
        {
            await bankConnectionProcess.Redirect(_redirectionService);
        });
    }

    [Test]
    public async Task RedirectingUser_WhenIsNotInTheRightStatus_BreaksBankConnectionProcessShouldKeepValidStatusTransitionsRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService);

        AssertBrokenRuleAsync<BankConnectionProcessShouldKeepValidStatusTransitionsRule>(async Task () =>
        {
            await bankConnectionProcess.Redirect(_redirectionService);
        });
    }
}
