using System.Net;
using System.Text.Json;
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
        httpResponseMessage.Content = new StringContent(JsonSerializer.Serialize(expectedResponseContent));

        var response = await Response.From(httpResponseMessage) as SuccessResponse;
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
        var expectedResponseError = new ResponseError("Error", "Some external error", "https://some-url.com", Guid.NewGuid());

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
        httpResponseMessage.Content = new StringContent(JsonSerializer.Serialize(expectedResponseError));

        var response = await Response.From(httpResponseMessage) as ErrorResponse;
        var error = response.Error;

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.IsSuccessful, Is.False);

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
