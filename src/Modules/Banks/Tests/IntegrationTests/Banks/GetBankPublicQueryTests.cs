using App.Modules.Banks.Application.Banks.Public.GetBank;
using App.Modules.Banks.Application.BanksSynchronisationProcessing.SynchroniseBanks;
using App.Modules.Banks.Application.Configuration.Data;
using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.IntegrationTests.SeedWork;
using Dapper;
using Npgsql;

namespace App.Modules.Banks.IntegrationTests.Banks;

[TestFixture]
public class GetBankPublicQueryTests : TestBase
{
    [Test]
    public async Task GetBankQuery_WhenBankExists_ReturnsBank()
    {
        await CreateBanksAsync();
        var bankId = await GetBankIdByExternalId("external-id-1");
        var bank = await BanksModule.ExecuteQueryAsync(new GetBankQuery(bankId));

        Assert.That(bank, Is.Not.Null);
        Assert.That(bank!.Id, Is.EqualTo(bankId));
        Assert.That(bank.Name, Is.EqualTo("Bank name 1"));
        Assert.That(bank.CountryCode, Is.EqualTo(Country.FakeCountry.Code));
        Assert.That(bank.IsBeta, Is.True);
        Assert.That(bank.DefaultLogoUrl, Is.EqualTo("https://external/banks/bank.svg"));
        Assert.That(bank.LogoUrl, Is.Null);
    }

    [Test]
    public async Task GetBankQuery_WhenBankIsDisabled_ReturnsBank()
    {
        await CreateBanksAsync();
        var bankId = await GetBankIdByExternalId("external-id-1");
        await DisableBankAsync(bankId);

        var bank = await BanksModule.ExecuteQueryAsync(new GetBankQuery(bankId));

        Assert.That(bank, Is.Null);
    }

    [Test]
    public async Task GetBankQuery_WhenBankDoesNotExist_ReturnsNull()
    {
        var bank = await BanksModule.ExecuteQueryAsync(new GetBankQuery(Guid.NewGuid()));

        Assert.That(bank, Is.Null);
    }

    private async Task<Guid> GetBankIdByExternalId(string externalId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT id FROM {DatabaseConfiguration.Schema.Name}.banks WHERE external_id = @externalId";

        return await connection.ExecuteScalarAsync<Guid>(sql, new { externalId });
    }

    private async Task CreateBanksAsync()
    {
        SaltEdgeHttpClientMocker.MockFetchProvidersSuccessfulResponse();
        await BanksModule.ExecuteCommandAsync(new SynchroniseBanksCommand(Guid.NewGuid()));
    }

    private async Task DisableBankAsync(Guid bankId)
    {
        await using var connection = new NpgsqlConnection(ConnectionString);

        var sql = $"UPDATE {DatabaseConfiguration.Schema.Name}.banks SET status='{BankStatus.Disabled.Value}' WHERE id = @id";

        await connection.ExecuteAsync(sql, new { id = bankId });
    }
}
