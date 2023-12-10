using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.UnitTests.BankConnectionProcessing;

[TestFixture]
public class BankConnectionProcessTests : UnitTestBase
{
    private static UserId _userId;

    private static BankId _bankId;

    private static WalletId _walletId;

    private static IBankConnectionProcessInitiationService _initiationService;

    [SetUp]
    public void SetUp()
    {
        _userId = new UserId(Guid.NewGuid());
        _bankId = new BankId(Guid.NewGuid());
        _walletId = new WalletId(Guid.NewGuid());
        _initiationService = Substitute.For<IBankConnectionProcessInitiationService>();
    }

    [Test]
    public async Task InitiatingBankConnectionProcess_IsSuccessful()
    {
        var bankConnectionProcess = await BankConnectionProcess.Initiate(_userId, _bankId, _walletId, WalletType.Debit, _initiationService);

        var processInitiatedDomainEvent = AssertPublishedDomainEvent<BankConnectionProcessInitiatedDomainEvent>(bankConnectionProcess);
        await _initiationService.Received(1).InitiateForAsync(_userId);
        Assert.That(processInitiatedDomainEvent.BankConnectionProcessId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(processInitiatedDomainEvent.UserId, Is.EqualTo(_userId));
        Assert.That(processInitiatedDomainEvent.BankId, Is.EqualTo(_bankId));
        Assert.That(processInitiatedDomainEvent.WalletId, Is.EqualTo(_walletId));
    }
}
