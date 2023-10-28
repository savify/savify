using WireMock.Server;

namespace App.Modules.Transactions.IntegrationTests.SeedData;

public class SaltEdgeHttpClientMocker
{
    private readonly WireMockServer _wireMock;

    public SaltEdgeHttpClientMocker(WireMockServer wireMock)
    {
        _wireMock = wireMock;
    }

    public void StopWireMockServer()
    {
        _wireMock.Stop();
    }
}
