using App.Integrations.SaltEdge.Requests;
using App.Integrations.SaltEdge.Responses;

namespace App.Integrations.SaltEdge.Client;

public interface ISaltEdgeHttpClient
{
    public Task<Response> SendAsync(Request request);
}
