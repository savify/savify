using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace App.IntegrationTests.SeedData;

public class SaltEdgeHttpClientMocker
{
    private readonly WireMockServer _wireMock = WireMockServer.Start();

    public string BaseUrl => _wireMock.Url!;

    public void StopWireMockServer()
    {
        _wireMock.Stop();
    }

    public void MockFetchCategoriesSuccessfulResponse()
    {
        var categoriesJson = new
        {
            data = new
            {
                business = new
                {
                    financials = new[] { "interest", "fees" }
                },

                personal = new
                {
                    income = new[] { "paycheck", "bonus" },
                    shopping = new[] { "clothing" },
                    home = new[] { "rent" }
                }
            }
        };

        _wireMock.Given(
                Request.Create()
                    .WithPath("/categories")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(categoriesJson));
    }

    public void MockFetchCategoriesErrorResponse()
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

        _wireMock.Given(
                Request.Create()
                    .WithPath("/categories")
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(400)
                    .WithBodyAsJson(errorJson));
    }
}
