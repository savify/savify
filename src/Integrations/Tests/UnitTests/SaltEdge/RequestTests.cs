using App.Integrations.SaltEdge.Requests;

namespace App.Integrations.UnitTests.SaltEdge;

[TestFixture]
public class RequestTests
{
    [Test]
    public void CreatingGetRequest_IsSuccessful()
    {
        var request = Request.Get("/some-path");

        Assert.That(request.Method, Is.EqualTo(HttpMethod.Get));
        Assert.That(request.Content, Is.Null);
        Assert.That(request.GetFullUrl("https://www.saltedge.com/api/v5"), Is.EqualTo("https://www.saltedge.com/api/v5" + "/some-path"));
    }

    [Test]
    public void CreatingGetRequest_WithQueryParameters_IsSuccessful()
    {
        var request = Request.Get("/some-path")
            .WithQueryParameter("foo", "bar")
            .WithQueryParameter("abc", 123);

        Assert.That(request.GetFullUrl("https://www.saltedge.com/api/v5"), Is.EqualTo("https://www.saltedge.com/api/v5" + "/some-path" + "?foo=bar&abc=123"));
    }

    [Test]
    public void CreatingGetRequest_WithQueryParametersBatch_IsSuccessful()
    {
        var queryParameters = new Dictionary<string, object>
        {
            {"foo", "bar"},
            {"abc", 123}
        };

        var request = Request.Get("/some-path")
            .WithQueryParameters(queryParameters);

        Assert.That(request.GetFullUrl("https://www.saltedge.com/api/v5"), Is.EqualTo("https://www.saltedge.com/api/v5" + "/some-path" + "?foo=bar&abc=123"));
    }

    [Test]
    public async Task CreatingPostRequest_IsSuccessful()
    {
        var content = new Dictionary<string, object>
        {
            {"foo", "bar"},
            {"abc", 123}
        };

        var request = Request.Post("/some-path")
            .WithContent(content);

        var requestContent = await request.Content.ReadAsStringAsync();

        Assert.That(request.Method, Is.EqualTo(HttpMethod.Post));
        Assert.That(requestContent, Is.EqualTo(@"{""data"":{""foo"":""bar"",""abc"":123}}"));
    }

    [Test]
    public async Task CreatingPutRequest_IsSuccessful()
    {
        var content = new Dictionary<string, object>
        {
            {"foo", "bar"},
            {"abc", 123}
        };

        var request = Request.Put("/some-path")
            .WithContent(content);

        var requestContent = await request.Content.ReadAsStringAsync();

        Assert.That(request.Method, Is.EqualTo(HttpMethod.Put));
        Assert.That(requestContent, Is.EqualTo(@"{""data"":{""foo"":""bar"",""abc"":123}}"));
    }

    [Test]
    public async Task CreatingPatchRequest_IsSuccessful()
    {
        var content = new Dictionary<string, object>
        {
            {"foo", "bar"},
            {"abc", 123}
        };

        var request = Request.Patch("/some-path")
            .WithContent(content);

        var requestContent = await request.Content.ReadAsStringAsync();

        Assert.That(request.Method, Is.EqualTo(HttpMethod.Patch));
        Assert.That(requestContent, Is.EqualTo(@"{""data"":{""foo"":""bar"",""abc"":123}}"));
    }

    [Test]
    public async Task CreatingDeleteRequest_IsSuccessful()
    {
        var request = Request.Delete("/some-path");

        Assert.That(request.Method, Is.EqualTo(HttpMethod.Delete));
    }
}
