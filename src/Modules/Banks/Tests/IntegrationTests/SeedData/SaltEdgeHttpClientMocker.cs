using App.Modules.Banks.Domain;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace App.Modules.Banks.IntegrationTests.SeedData;

public class SaltEdgeHttpClientMocker(WireMockServer wireMock)
{
    public void StopWireMockServer()
    {
        wireMock.Stop();
    }

    public void MockFetchProvidersSuccessfulResponse()
    {
        var banksJson = new
        {
            data = new object[]
            {
                new
                {
                    code = "external-id-1",
                    name = "Bank name 1",
                    status = "active",
                    country_code = Country.FakeCountry.Code,
                    regulated = true,
                    max_consent_days = 90,
                    logo_url = "https://external/banks/bank.svg"
                },
                new
                {
                    code = "external-id-2",
                    name = "Bank name 2",
                    status = "active",
                    country_code = Country.FakeCountry.Code,
                    regulated = false,
                    max_consent_days = 90,
                    logo_url = "https://external/banks/bank.svg"
                }
            }
        };

        wireMock.Given(
                Request.Create()
                    .WithPath("/providers")
                    .WithParam("include_fake_providers")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(banksJson));
    }

    public void MockFetchUpdatedProvidersSuccessfulResponse()
    {
        var banksJson = new
        {
            data = new object[]
            {
                new
                {
                    code = "external-id-1",
                    name = "Bank name 1",
                    status = "inactive",
                    country_code = Country.FakeCountry.Code,
                    regulated = false,
                    max_consent_days = 30,
                    logo_url = "https://external/banks/bank-new.svg"
                }
            }
        };

        wireMock.Given(
                Request.Create()
                    .WithPath("/providers")
                    .WithParam("include_fake_providers")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(banksJson));
    }

    public void MockFetchProvidersErrorResponse()
    {
        var errorJson = new
        {
            error = new
            {
                Class = "SomeError",
                message = "Some error message",
                documentation_url = "https://external/",
                request_id = Guid.NewGuid().ToString()
            },
        };

        wireMock.Given(
                Request.Create()
                    .WithPath("/providers")
                    .WithParam("include_fake_providers", true)
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(400)
                    .WithBodyAsJson(errorJson));
    }
}
