using App.Modules.Banks.Application.Banks.GetBanks;
using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using App.Modules.Banks.IntegrationTests.SeedWork;
using App.Modules.Notifications.Application.Contracts;
using Dapper;
using Npgsql;

namespace App.Modules.Banks.IntegrationTests.BanksSynchronisationProcessing;

[TestFixture]
public class SynchroniseBanksRecurringCommandTests : TestBase
{
    [Test]
    public async Task SynchroniseBanksRecurringCommand_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();

        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksRecurringCommand());

        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());
        Assert.That(banks.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task SynchroniseBanksRecurringCommand_WhenErrorAtProviderOccurred_WillReturnFailedStatus_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersErrorResponse();

        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksRecurringCommand());

        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());
        Assert.That(banks.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task SynchroniseBanksRecurringCommand_WhenSomeBankWasUpdated_WillUpdateThisBank_Test()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();
        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksRecurringCommand());

        var lastProcessFinishedAt = await GetLastBanksSyncProcessFinishedAt();
        SaltEdgeHttpClientMocker.MockFetchUpdatedProvidersSuccessfulResponse(lastProcessFinishedAt);

        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());
        var updatedBank = banks.Single(b => b.Name == "Bank name 1");

        Assert.That(banks.Count, Is.EqualTo(2));
        Assert.That(updatedBank.Status, Is.EqualTo(BankStatus.Disabled.Value));
        Assert.That(updatedBank.IsRegulated, Is.False);
        Assert.That(updatedBank.MaxConsentDays, Is.EqualTo(30));
        Assert.That(updatedBank.DefaultLogoUrl, Is.EqualTo("https://external/banks/bank-new.svg"));
    }

    private async Task<DateTime> GetLastBanksSyncProcessFinishedAt()
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = @"SELECT finished_at FROM banks.banks_synchronisation_processes WHERE status = @FinishedStatus";

        return await connection.QuerySingleAsync<DateTime>(
            sql,
            new { FinishedStatus = BanksSynchronisationProcessStatus.Finished.Value });
    }
}
