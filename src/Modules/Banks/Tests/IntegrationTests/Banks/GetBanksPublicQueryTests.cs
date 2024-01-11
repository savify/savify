using App.Modules.Banks.Application.Banks.Public.GetBanks;
using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.Banks.IntegrationTests.Banks;

[TestFixture]
public class GetBanksPublicQueryTests : TestBase
{
    [Test]
    public async Task GetBanksQuery_ReturnsListOfBanks()
    {
        await CreateBanksAsync();
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(banks.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetBanksQuery_DoesNotReturnBanks_ThatAreDisabled()
    {
        await CreateBanksAsync();
        await DisableBankAsync("external-id-1");
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(banks.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetBanksQuery_WhenThereAreNoBanks_ReturnsEmptyList()
    {
        var banks = await BanksModule.ExecuteQueryAsync(new GetBanksQuery());

        Assert.That(banks, Is.Empty);
    }

    private async Task CreateBanksAsync()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();
        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));
    }

    private async Task DisableBankAsync(string externalBankId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"UPDATE {DatabaseConfiguration.Schema.Name}.banks SET status='{BankStatus.Disabled.Value}' WHERE external_id = @externalBankId";

        await connection.ExecuteAsync(sql, new { externalBankId });
    }
}
