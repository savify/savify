using System.Net;
using System.Text.Json;
using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Configuration;
using App.Integrations.SaltEdge.Requests;
using App.Integrations.SaltEdge.Responses;
using App.Integrations.UnitTests.SeedWork;
using Serilog;

namespace App.Integrations.UnitTests.SaltEdge;

[TestFixture]
public class SaltEdgeHttpClientTests
{
    private SaltEdgeClientConfiguration _configuration;

    private ILogger _logger;

    [SetUp]
    public void SetUp()
    {
        _configuration = new SaltEdgeClientConfiguration();
        _configuration.BaseUrl = "https://www.saltedge.com/api/v5";
        _configuration.AppId = "app-id";
        _configuration.AppSecret = "app-secret";

        _logger = Substitute.For<ILogger>();
    }

    [Test]
    public async Task SendsRequest_And_ReceivesSuccessResponse()
    {
        var expectedResponseContent = new ContentDto("bar", 123);

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponseContent));
        var httpClient = new HttpClient(httpMessageHandler);

        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient().Returns(httpClient);

        var client = new SaltEdgeHttpClient(_configuration, httpClientFactory, _logger);
        var request = Request.Get("/some-path");

        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessful, Is.True);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response, Is.InstanceOf<SuccessResponse>());

        var content = (response as SuccessResponse).Content.As<ContentDto>();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Foo, Is.EqualTo(expectedResponseContent.Foo));
        Assert.That(content.Abc, Is.EqualTo(expectedResponseContent.Abc));
    }

    [Test]
    public async Task SendsRequest_And_ReceivesErrorResponse()
    {
        var expectedResponseError = new ResponseError("Error", "Some external error", "https://some-url.com", Guid.NewGuid());

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.BadRequest, JsonSerializer.Serialize(expectedResponseError));
        var httpClient = new HttpClient(httpMessageHandler);

        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        httpClientFactory.CreateClient().Returns(httpClient);

        var client = new SaltEdgeHttpClient(_configuration, httpClientFactory, _logger);
        var request = Request.Get("/some-path");

        var response = await client.SendAsync(request);

        Assert.That(response.IsSuccessful, Is.False);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response, Is.InstanceOf<ErrorResponse>());

        var error = (response as ErrorResponse).Error;
        Assert.That(error, Is.Not.Null);
        Assert.That(error.Class, Is.EqualTo(expectedResponseError.Class));
        Assert.That(error.Message, Is.EqualTo(expectedResponseError.Message));
        Assert.That(error.DocumentationUrl, Is.EqualTo(expectedResponseError.DocumentationUrl));
        Assert.That(error.RequestId, Is.EqualTo(expectedResponseError.RequestId));
    }

    class ContentDto
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
