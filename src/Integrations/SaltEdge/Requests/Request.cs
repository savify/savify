using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace App.Integrations.SaltEdge.Requests;

public abstract class Request
{
    public HttpMethod Method { get; }

    private string Path { get; }

    private IDictionary<string, object> _queryParameters = new Dictionary<string, object>();

    public HttpContent? Content { get; protected set; }

    protected Request(HttpMethod method, string path)
    {
        Method = method;
        Path = path;
    }

    public Request WithQueryParameter(string key, object value)
    {
        _queryParameters[key] = value;

        return this;
    }

    public Request WithQueryParameters(IDictionary<string, object> queryParameters)
    {
        _queryParameters = queryParameters;

        return this;
    }

    public string GetFullUrl(string url)
    {
        if (_queryParameters.Any())
        {
            var queryParameters = _queryParameters.Select(parameter => $"{parameter.Key}={parameter.Value}");

            return url + Path + "?" + string.Join("&", queryParameters);
        }

        return url + Path;
    }

    public static GetRequest Get(string path)
    {
        return new GetRequest(path);
    }

    public static PostRequest Post(string path)
    {
        return new PostRequest(path);
    }

    public static PatchRequest Patch(string path)
    {
        return new PatchRequest(path);
    }

    public static PutRequest Put(string path)
    {
        return new PutRequest(path);
    }

    public static DeleteRequest Delete(string path)
    {
        return new DeleteRequest(path);
    }
}

public class GetRequest : Request
{
    public GetRequest(string path) : base(HttpMethod.Get, path)
    {
    }
}

public class PostRequest : Request
{
    public PostRequest(string path) : base(HttpMethod.Post, path)
    { }

    public Request WithContent(object content)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return this;
    }
}

public class PatchRequest : Request
{
    public PatchRequest(string path) : base(HttpMethod.Patch, path)
    {
    }

    public Request WithContent(object content)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return this;
    }
}

public class PutRequest : Request
{
    public PutRequest(string path) : base(HttpMethod.Put, path)
    {
    }

    public Request WithContent(object content)
    {
        Content = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        return this;
    }
}

public class DeleteRequest : Request
{
    public DeleteRequest(string path) : base(HttpMethod.Delete, path)
    {
    }
}
