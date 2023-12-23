using App.Integrations.SaltEdge.Client;
using App.Integrations.SaltEdge.Requests;
using App.Modules.Categories.Infrastructure.Integrations.SaltEdge.Categories;

namespace App.Modules.Categories.Infrastructure.Integrations.SaltEdge;

public class SaltEdgeIntegrationService(ISaltEdgeHttpClient client) : ISaltEdgeIntegrationService
{
    public async Task<SaltEdgeCategories> FetchCategoriesAsync()
    {
        var request = Request.Get("categories");

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw new SaltEdgeIntegrationException(response.Error!.Message);
        }

        var categories = response.Content?.As<SaltEdgeCategories>();

        return categories ?? SaltEdgeCategories.Empty();
    }
}
