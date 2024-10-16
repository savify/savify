using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.BankConnections.Events;
using App.Modules.FinanceTracking.Domain.BankConnections.Rules;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;

namespace App.Modules.FinanceTracking.UnitTests.BankConnections;

[TestFixture]
public class BankConnectionTests : UnitTestBase
{
    [Test]
    public void CreatingBankConnection_FromBankConnectionProcess_IsSuccessful()
    {
        var bankConnectionProcessId = new BankConnectionProcessId(Guid.NewGuid());
        var bankId = new BankId(Guid.NewGuid());
        var userId = new UserId(Guid.NewGuid());

        var bankConnection = BankConnection.CreateFromBankConnectionProcess(bankConnectionProcessId, bankId, userId, new Consent(DateTime.MaxValue));

        var bankConnectionCreatedDomainEvent = AssertPublishedDomainEvent<BankConnectionCreatedDomainEvent>(bankConnection);
        Assert.That(bankConnectionCreatedDomainEvent.BankConnectionId.Value, Is.EqualTo(bankConnectionProcessId.Value));
        Assert.That(bankConnectionCreatedDomainEvent.BankId, Is.EqualTo(bankId));
        Assert.That(bankConnectionCreatedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(bankConnection.HasMultipleBankAccounts, Is.False);
    }

    [Test]
    public void AddingBankAccount_ToBankConnection_IsSuccessful()
    {
        var bankConnection = BankConnection.CreateFromBankConnectionProcess(
            new BankConnectionProcessId(Guid.NewGuid()),
            new BankId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new Consent(DateTime.MaxValue));

        bankConnection.AddBankAccount("123", "Test account", 1000, Currency.From("USD"));

        var addedBankAccount = bankConnection.GetSingleBankAccount();
        Assert.That(addedBankAccount, Is.InstanceOf<BankAccount>());
    }

    [Test]
    public void AddingTwoOrMoreBankAccounts_ToBankConnection_IsSuccessful()
    {
        var bankConnection = BankConnection.CreateFromBankConnectionProcess(
            new BankConnectionProcessId(Guid.NewGuid()),
            new BankId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new Consent(DateTime.MaxValue));

        bankConnection.AddBankAccount("123", "Test account 1", 1000, Currency.From("USD"));
        bankConnection.AddBankAccount("456", "Test account 2", 1000, Currency.From("PLN"));

        Assert.That(bankConnection.HasMultipleBankAccounts, Is.True);
    }

    [Test]
    public void GettingSingleBankAccount_WhenConnectionHasNoAccounts_BreaksBankConnectionMustHaveOnlyOneAccountRule()
    {
        var bankConnection = BankConnection.CreateFromBankConnectionProcess(
            new BankConnectionProcessId(Guid.NewGuid()),
            new BankId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new Consent(DateTime.MaxValue));

        AssertBrokenRule<BankConnectionMustHaveOnlyOneAccountRule>(() =>
        {
            bankConnection.GetSingleBankAccount();
        });
    }

    [Test]
    public void GettingSingleBankAccount_WhenConnectionHasMultipleAccounts_BreaksBankConnectionMustHaveOnlyOneAccountRule()
    {
        var bankConnection = BankConnection.CreateFromBankConnectionProcess(
            new BankConnectionProcessId(Guid.NewGuid()),
            new BankId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new Consent(DateTime.MaxValue));

        bankConnection.AddBankAccount("123", "Test account 1", 1000, Currency.From("USD"));
        bankConnection.AddBankAccount("456", "Test account 2", 1000, Currency.From("PLN"));

        AssertBrokenRule<BankConnectionMustHaveOnlyOneAccountRule>(() =>
        {
            bankConnection.GetSingleBankAccount();
        });
    }

    [Test]
    public void GettingBankAccountById_WhenDoesNotExist_BreaksBankConnectionShouldHaveBankAccountWithGivenIdRule()
    {
        var bankConnection = BankConnection.CreateFromBankConnectionProcess(
            new BankConnectionProcessId(Guid.NewGuid()),
            new BankId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new Consent(DateTime.MaxValue));

        AssertBrokenRule<BankConnectionShouldHaveBankAccountWithGivenIdRule>(() =>
        {
            bankConnection.GetBankAccountById(new BankAccountId(Guid.NewGuid()));
        });
    }
}
