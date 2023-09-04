using App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;
using App.Modules.Wallets.Application.Wallets.DebitWallets.ConnectBankAccountToDebitWallet;
using App.Modules.Wallets.IntegrationTests.SeedWork;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace App.Modules.Wallets.IntegrationTests.DebitWallets;

[TestFixture]
public class ConnectBankAccountToDebitWalletTests : TestBase
{
    [Test]
    public async Task ConnectBankAccountToDebitWalletCommand_Test()
    {
        var userId = Guid.NewGuid();
        var bankId = Guid.NewGuid();
        var walletId = await AddDebitWalletFor(userId);

        var customerId = "1087222023010130820";
        var providerCode = "fakebank_interactive_xf";
        var expectedRedirectUrl = "https://www.saltedge.com/connect?token=GENERATED_TOKEN";

        MockCreateCustomerSuccessfulResponse(userId, customerId);
        MockCreateConnectSessionSuccessfulResponse(customerId, providerCode, expectedRedirectUrl);

        var redirectUrl = await WalletsModule.ExecuteCommandAsync(new ConnectBankAccountToDebitWalletCommand(userId, walletId, bankId));

        Assert.That(redirectUrl, Is.EqualTo(expectedRedirectUrl));
    }

    private void MockCreateCustomerSuccessfulResponse(Guid userId, string customerId)
    {
        WireMock.Given(
                Request.Create()
                    .WithPath("/customers")
                    .WithBodyAsJson(new { data = new { identifier = userId.ToString() } })
                    .UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new
                    {
                        data = new
                        {
                            id = customerId,
                            identifier = userId.ToString()
                        }
                    }));
    }

    private void MockCreateConnectSessionSuccessfulResponse(string customerId, string providerCode, string expectedConnectUrl)
    {
        WireMock.Given(
                Request.Create()
                    .WithPath("/connect_sessions/create")
                    .WithBody(new JsonPartialMatcher(new
                    {
                        data = new
                        {
                            customer_id = customerId,
                            provider_code = providerCode
                        }
                    }))
                    .UsingPost()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(201)
                    .WithBodyAsJson(new
                    {
                        data = new
                        {
                            expires_at = "2023-08-22T07:58:14Z",
                            connect_url = expectedConnectUrl
                        }
                    }));
    }

    private async Task<Guid> AddDebitWalletFor(Guid userId)
    {
        var command = new AddNewDebitWalletCommand(
            userId,
            "Debit wallet",
            "PLN",
            1000,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        return await WalletsModule.ExecuteCommandAsync(command);
    }
}
