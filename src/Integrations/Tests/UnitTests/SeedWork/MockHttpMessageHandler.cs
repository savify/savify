using System.Net;

namespace App.Integrations.UnitTests.SeedWork;

public class MockHttpMessageHandler : HttpMessageHandler
{
    public string? Input { get; private set; }

    public int NumberOfCalls { get; private set; }

    private readonly HttpStatusCode _statusCode;

    private readonly string _serializedResponse;

    public MockHttpMessageHandler(HttpStatusCode statusCode, string serializedResponse)
    {
        _statusCode = statusCode;
        _serializedResponse = serializedResponse;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        NumberOfCalls++;

        if (request.Content is not null)
        {
            Input = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        return new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = new StringContent(_serializedResponse),
        };
    }
}
