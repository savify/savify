using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Integrations.SaltEdge.Responses;

namespace App.Integrations.UnitTests.SaltEdge;

[TestFixture]
public class ResponseTests
{
    [Test]
    public async Task CreatingResponse_FromHttpResponseMessage_WithSuccessStatusCode_CreatesSuccessResponse()
    {
        var expectedResponseContent = new ContentDto("bar", 123);

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Content = new StringContent(JsonSerializer.Serialize(new Dictionary<string, object>
        {
            {"data", expectedResponseContent}
        }));

        var response = await Response.From(httpResponseMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.IsSuccessful, Is.True);

        var content = response.Content.As<ContentDto>();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Foo, Is.EqualTo("bar"));
        Assert.That(content.Abc, Is.EqualTo(123));

    }

    [Test]
    public async Task CreatingResponse_FromHttpResponseMessage_WithNotSuccessStatusCode_CreatesErrorResponse()
    {
        var requestId = Guid.NewGuid();
        var expectedResponse = new Dictionary<string, object> { { "error", new Dictionary<string, object>
        {
            {"class", "Error"},
            {"message", "Some external error"},
            {"documentation_url", "https://some-url.com"},
            {"request_id", requestId}
        } } };

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        httpResponseMessage.Content = new StringContent(JsonSerializer.Serialize(expectedResponse));

        var response = await Response.From(httpResponseMessage);
        var error = response.Error;

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.IsSuccessful, Is.False);

        Assert.That(error, Is.Not.Null);
        Assert.That(error.Class, Is.EqualTo("Error"));
        Assert.That(error.Message, Is.EqualTo("Some external error"));
        Assert.That(error.DocumentationUrl, Is.EqualTo("https://some-url.com"));
        Assert.That(error.RequestId, Is.EqualTo(requestId));

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
