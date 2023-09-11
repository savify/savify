using System.Net;
using System.Text.Json;
using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Integrations.UnitTests.SeedWork;
using Serilog;

namespace App.Integrations.UnitTests.SaltEdge;

[TestFixture]
public class SaltEdgeHttpClientTests
{
    private readonly string _baseUrl = "https://www.saltedge.com/api/v5/";

    private ILogger _logger;

    [SetUp]
    public void SetUp()
    {
        _logger = Substitute.For<ILogger>();
    }

    [Test]
    public async Task SendsRequest_And_ReceivesSuccessResponse()
    {
        var expectedResponseContent = new ContentDto("bar", 123);

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(new Dictionary<string, object>
        {
            {"data", expectedResponseContent}
        }));
        var httpClient = new HttpClient(httpMessageHandler);
        httpClient.BaseAddress = new Uri(_baseUrl);

        var client = new SaltEdgeHttpClient(httpClient, _logger);
        var request = Request.Get("/some-path");

        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessful, Is.True);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = response.Content.As<ContentDto>();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Foo, Is.EqualTo(expectedResponseContent.Foo));
        Assert.That(content.Abc, Is.EqualTo(expectedResponseContent.Abc));
    }

    [Test]
    public async Task SendsRequest_And_ReceivesErrorResponse()
    {
        var requestId = Guid.NewGuid();
        var expectedResponse = new Dictionary<string, object> { { "error", new Dictionary<string, object>
        {
            {"class", "Error"},
            {"message", "Some external error"},
            {"documentation_url", "https://some-url.com"},
            {"request_id", requestId}
        } } };

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, JsonSerializer.Serialize(expectedResponse));
        var httpClient = new HttpClient(httpMessageHandler);
        httpClient.BaseAddress = new Uri(_baseUrl);

        var client = new SaltEdgeHttpClient(httpClient, _logger);
        var request = Request.Get("/some-path");

        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessful, Is.False);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        var error = response.Error;
        Assert.That(error, Is.Not.Null);
        Assert.That(error.Class, Is.EqualTo("Error"));
        Assert.That(error.Message, Is.EqualTo("Some external error"));
        Assert.That(error.DocumentationUrl, Is.EqualTo("https://some-url.com"));
        Assert.That(error.RequestId, Is.EqualTo(requestId));
    }

    private class ContentDto
    {
        public string Foo { get; }

        public int Abc { get; }

        public ContentDto(string foo, int abc)
        {
            Foo = foo;
            Abc = abc;
        }
    }
}
