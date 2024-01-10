using App.BuildingBlocks.Domain.Results;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Events;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing.Services;

namespace App.Modules.Banks.UnitTests.BanksSynchronisationProcessing;

[TestFixture]
public class BanksSynchronisationProcessTests : UnitTestBase
{
    [Test]
    public async Task StartingBanksSynchronisationProcess_ThatWasNotPerformedBefore_WillFinishWithSuccess()
    {
        var banksSynchronisationService = Substitute.For<IBanksSynchronisationService>();
        banksSynchronisationService
            .SynchroniseAsync(Arg.Any<BanksSynchronisationProcessId>())
            .Returns(Result.Success);

        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.InternalCommand,
            banksSynchronisationService
            );

        var processStartedDomainEvent = AssertPublishedDomainEvent<BanksSynchronisationProcessStartedDomainEvent>(banksSynchronisationProcess);
        var processFinishedDomainEvent = AssertPublishedDomainEvent<BanksSynchronisationProcessFinishedDomainEvent>(banksSynchronisationProcess);

        await banksSynchronisationService.Received(1).SynchroniseAsync(banksSynchronisationProcess.Id);
        Assert.That(banksSynchronisationProcess.Status, Is.EqualTo(BanksSynchronisationProcessStatus.Finished));
        Assert.That(processStartedDomainEvent.BanksSynchronisationProcessId, Is.EqualTo(banksSynchronisationProcess.Id));
        Assert.That(processStartedDomainEvent.InitiatedBy.Type, Is.EqualTo(BanksSynchronisationProcessInitiatorType.InternalCommand));
        Assert.That(processFinishedDomainEvent.BanksSynchronisationProcessId, Is.EqualTo(banksSynchronisationProcess.Id));
    }

    [Test]
    public async Task BanksSynchronisationProcess_WhenSynchronisationHasFailed_WillFail()
    {
        var banksSynchronisationService = Substitute.For<IBanksSynchronisationService>();
        banksSynchronisationService
            .SynchroniseAsync(Arg.Any<BanksSynchronisationProcessId>())
            .Returns(Result.Error);

        var banksSynchronisationProcess = await BanksSynchronisationProcess.Start(
            BanksSynchronisationProcessInitiator.InternalCommand,
            banksSynchronisationService
        );

        var processStartedDomainEvent = AssertPublishedDomainEvent<BanksSynchronisationProcessStartedDomainEvent>(banksSynchronisationProcess);
        var processFailedDomainEvent = AssertPublishedDomainEvent<BanksSynchronisationProcessFailedDomainEvent>(banksSynchronisationProcess);

        await banksSynchronisationService.Received(1).SynchroniseAsync(banksSynchronisationProcess.Id);
        Assert.That(banksSynchronisationProcess.Status, Is.EqualTo(BanksSynchronisationProcessStatus.Failed));
        Assert.That(processStartedDomainEvent.BanksSynchronisationProcessId, Is.EqualTo(banksSynchronisationProcess.Id));
        Assert.That(processStartedDomainEvent.InitiatedBy.Type, Is.EqualTo(BanksSynchronisationProcessInitiatorType.InternalCommand));
        Assert.That(processFailedDomainEvent.BanksSynchronisationProcessId, Is.EqualTo(banksSynchronisationProcess.Id));
    }
}
