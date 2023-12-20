using System.Net;

namespace App.Integrations.UnitTests.SeedWork;

public class MockHttpMessageHandler(HttpStatusCode statusCode, string serializedResponse) : HttpMessageHandler
{
    public string? Input { get; private set; }

    public int NumberOfCalls { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        NumberOfCalls++;

        if (request.Content is not null)
        {
            Input = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        return new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = new StringContent(serializedResponse),
        };
    }
}
