using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;
using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing.Events;
using NSubstitute.ExceptionExtensions;

namespace App.Modules.Categories.UnitTests.CategoriesSynchronisationProcessing;

[TestFixture]
public class CategoriesSynchronisationProcessTests : UnitTestBase
{
    [Test]
    public async Task StartingCategoriesSynchronisationProcess_WillFinishWithSuccess()
    {
        var categoriesSynchronisationService = Substitute.For<ICategoriesSynchronisationService>();

        var categoriesSynchronisationProcess = await CategoriesSynchronisationProcess.Start(categoriesSynchronisationService);

        var processStartedDomainEvent = AssertPublishedDomainEvent<CategoriesSynchronisationProcessStartedDomainEvent>(categoriesSynchronisationProcess);
        var processFinishedDomainEvent = AssertPublishedDomainEvent<CategoriesSynchronisationProcessFinishedDomainEvent>(categoriesSynchronisationProcess);

        await categoriesSynchronisationService.Received(1).SynchroniseAsync();
        Assert.That(categoriesSynchronisationProcess.Status, Is.EqualTo(CategoriesSynchronisationProcessStatus.Finished));
        Assert.That(processStartedDomainEvent.CategoriesSynchronisationProcessId, Is.EqualTo(categoriesSynchronisationProcess.Id));
        Assert.That(processFinishedDomainEvent.CategoriesSynchronisationProcessId, Is.EqualTo(categoriesSynchronisationProcess.Id));
    }

    [Test]
    public async Task StartingCategoriesSynchronisationProcess_WhenCategoriesSynchronisationProcessExceptionWasThrown_WillFail()
    {
        var categoriesSynchronisationService = Substitute.For<ICategoriesSynchronisationService>();
        categoriesSynchronisationService.SynchroniseAsync()
            .ThrowsAsync(new CategoriesSynchronisationProcessException("Some sync error message"));

        var categoriesSynchronisationProcess = await CategoriesSynchronisationProcess.Start(categoriesSynchronisationService);

        var processStartedDomainEvent = AssertPublishedDomainEvent<CategoriesSynchronisationProcessStartedDomainEvent>(categoriesSynchronisationProcess);
        var processFailedDomainEvent = AssertPublishedDomainEvent<CategoriesSynchronisationProcessFailedDomainEvent>(categoriesSynchronisationProcess);

        await categoriesSynchronisationService.Received(1).SynchroniseAsync();
        Assert.That(categoriesSynchronisationProcess.Status, Is.EqualTo(CategoriesSynchronisationProcessStatus.Failed));
        Assert.That(processStartedDomainEvent.CategoriesSynchronisationProcessId, Is.EqualTo(categoriesSynchronisationProcess.Id));
        Assert.That(processFailedDomainEvent.CategoriesSynchronisationProcessId, Is.EqualTo(categoriesSynchronisationProcess.Id));
    }
}
