using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace App.Modules.Categories.IntegrationTests.SeedData;

public class SaltEdgeHttpClientMocker(WireMockServer wireMock)
{
    public void StopWireMockServer()
    {
        wireMock.Stop();
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

        wireMock.Given(
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

        wireMock.Given(
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
