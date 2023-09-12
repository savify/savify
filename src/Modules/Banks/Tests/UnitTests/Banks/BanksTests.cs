using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.Events;
using App.Modules.Banks.Domain.ExternalProviders;

namespace App.Modules.Banks.UnitTests.Banks;

[TestFixture]
public class BanksTests : UnitTestBase
{
    [Test]
    public void AddingNewBank_IsSuccessful()
    {
        var bank = Bank.AddNew(
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
    }
}
