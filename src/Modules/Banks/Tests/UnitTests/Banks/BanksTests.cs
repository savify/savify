using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.Events;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.ExternalProviders;

namespace App.Modules.Banks.UnitTests.Banks;

[TestFixture]
public class BanksTests : UnitTestBase
{
    [Test]
    public void AddingNewBank_IsSuccessful()
    {
        var bankSynchronisationProcessId = new BanksSynchronisationProcessId(Guid.NewGuid());
        var bank = Bank.AddNew(
            bankSynchronisationProcessId,
            ExternalProviderName.SaltEdge,
            "external-id",
            "Bank name",
            Country.FakeCountry,
            BankStatus.Enabled,
            true,
            null,
            "https://cdn.savify.localhost/logos/banks/bank.png");

        var bankAddedDomainEvent = AssertPublishedDomainEvent<BankAddedDomainEvent>(bank);
        Assert.That(bankAddedDomainEvent.BankId, Is.EqualTo(bank.Id));
        Assert.That(bankAddedDomainEvent.ExternalProviderName, Is.EqualTo(ExternalProviderName.SaltEdge));
        Assert.That(bankAddedDomainEvent.ExternalId, Is.EqualTo("external-id"));
        Assert.That(bank.IsEnabled, Is.True);
        Assert.That(bank.IsFake, Is.True);
        Assert.That(bank.IsInBeta, Is.False);
        Assert.That(bank.LastBanksSynchronisationProcessId, Is.EqualTo(bankSynchronisationProcessId));
    }

    [Test]
    public void UpdatingExistingBank_IsSuccessful()
    {
        var bankSynchronisationProcessId = new BanksSynchronisationProcessId(Guid.NewGuid());
        var bank = Bank.AddNew(
            bankSynchronisationProcessId,
            ExternalProviderName.SaltEdge,
            "external-id",
            "Bank name",
            Country.FakeCountry,
            BankStatus.Enabled,
            true,
            null,
            "https://cdn.savify.localhost/logos/banks/bank.png");

        var newBankSynchronisationProcessId = new BanksSynchronisationProcessId(Guid.NewGuid());
        bank.Update(
            newBankSynchronisationProcessId,
            "New name",
            wasDisabled: true,
            isRegulated: false,
            30,
            "https://cdn.savify.localhost/logos/banks/new-bank.png");

        var domainEvent = AssertPublishedDomainEvent<BankUpdatedDomainEvent>(bank);
        Assert.That(domainEvent.BankId, Is.EqualTo(bank.Id));
        Assert.That(domainEvent.BanksSynchronisationProcessId, Is.EqualTo(newBankSynchronisationProcessId));
        Assert.That(domainEvent.Name, Is.EqualTo("New name"));
        Assert.That(domainEvent.WasDisabled, Is.True);
        Assert.That(domainEvent.IsRegulated, Is.False);
        Assert.That(domainEvent.MaxConsentDays, Is.EqualTo(30));
        Assert.That(domainEvent.DefaultLogoUrl, Is.EqualTo("https://cdn.savify.localhost/logos/banks/new-bank.png"));
        Assert.That(bank.LastBanksSynchronisationProcessId, Is.EqualTo(newBankSynchronisationProcessId));
        Assert.That(bank.IsEnabled, Is.False);
    }
}
