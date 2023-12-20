using WireMock.Server;

namespace App.Modules.Categories.IntegrationTests.SeedData;

public class SaltEdgeHttpClientMocker(WireMockServer wireMock)
{
    public void StopWireMockServer()
    {
        wireMock.Stop();
    }
}
