using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Rules;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing;

[TestFixture]
public class BankConnectionProcessBankAccountChoosingTests : UnitTestBase
{
    private static UserId _userId = null!;

    private static BankId _bankId = null!;

    private static WalletId _walletId = null!;

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
    public async Task ChoosingBankAccount_ToConnectToWallet_IsSuccessful()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService);

        var bankConnectionStub = BankConnection.CreateFromBankConnectionProcess(bankConnectionProcess.Id, _bankId, _userId, new Consent(DateTime.MaxValue));
        bankConnectionStub.AddBankAccount("123", "Test Account 1", 100, Currency.From("USD"));
        bankConnectionStub.AddBankAccount("456", "Test Account 2", 100, Currency.From("PLN"));

        _connectionCreationService
            .CreateConnection(bankConnectionProcess.Id, _userId, _bankId, "123456")
            .Returns(bankConnectionStub);

        var connectionResult = await bankConnectionProcess.CreateConnection("123456", _connectionCreationService, _bankAccountConnector);

        var accountId = new BankAccountId(Guid.NewGuid());
        await bankConnectionProcess.ChooseBankAccount(accountId, _bankAccountConnector);

        var completedDomainEvent = AssertPublishedDomainEvent<BankConnectionProcessCompletedDomainEvent>(bankConnectionProcess);
        Assert.That(completedDomainEvent.BankConnectionProcessId, Is.EqualTo(bankConnectionProcess.Id));

        Assert.That(connectionResult.IsSuccess, Is.True);
        var connection = connectionResult.Success;

        Assert.That(connection.Id.Value, Is.EqualTo(bankConnectionProcess.Id.Value));

        await _bankAccountConnector.Received(1).ConnectBankAccountToWallet(_walletId, WalletType.Debit, connection.Id, accountId);
    }

    [Test]
    public async Task ChoosingBankAccount_WhenBankConnectionProcessIsInFinalStatus_BreaksCannotOperateOnBankConnectionProcessWithFinalStatusRule()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var redirection = new Redirection("https://redirect-url.com/connect", DateTime.MaxValue);
        _redirectionService.Redirect(bankConnectionProcess.Id, _userId, _bankId).Returns(redirection);

        await bankConnectionProcess.Redirect(_redirectionService);

        var bankConnectionStub = BankConnection.CreateFromBankConnectionProcess(bankConnectionProcess.Id, _bankId, _userId, new Consent(DateTime.MaxValue));
        bankConnectionStub.AddBankAccount("123", "Test Account 1", 100, Currency.From("USD"));

        _connectionCreationService
            .CreateConnection(bankConnectionProcess.Id, _userId, _bankId, "123456")
            .Returns(bankConnectionStub);

        await bankConnectionProcess.CreateConnection("123456", _connectionCreationService, _bankAccountConnector);

        AssertBrokenRuleAsync<CannotOperateOnBankConnectionProcessWithFinalStatusRule>(async Task () =>
        {
            await bankConnectionProcess.ChooseBankAccount(new BankAccountId(Guid.NewGuid()), _bankAccountConnector);
        });
    }
}
