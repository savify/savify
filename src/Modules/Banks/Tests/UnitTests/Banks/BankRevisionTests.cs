using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.Banks.BankRevisions.Events;

namespace App.Modules.Banks.UnitTests.Banks;

[TestFixture]
public class BankRevisionTests : UnitTestBase
{
    [Test]
    public void CreatingBankRevision_IsSuccessful()
    {
        var bankId = new BankId(Guid.NewGuid());
        var revision = BankRevision.CreateNew(
            bankId,
            new BankRevisionCreator(BankRevisionCreatorType.SynchronisationProcess, Guid.NewGuid()),
            BankRevisionType.Added,
            "Bank name 1",
            BankStatus.Beta,
            true,
            null,
            null,
            "https://cdn.savify.localhost/logos/banks/bank.png");

        var domainEvent = AssertPublishedDomainEvent<BankRevisionCreatedDomainEvent>(revision);
        Assert.That(domainEvent.BankRevisionId, Is.EqualTo(revision.Id));
        Assert.That(domainEvent.BankId, Is.EqualTo(bankId));
        Assert.That(domainEvent.BankRevisionType, Is.EqualTo(BankRevisionType.Added));
    }
}
