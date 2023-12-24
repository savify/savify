using App.Modules.Categories.Infrastructure.Integrations.SaltEdge.Categories;

namespace App.Modules.Categories.Infrastructure.Integrations.SaltEdge;

public interface ISaltEdgeIntegrationService
{
    public Task<SaltEdgeCategories> FetchCategoriesAsync();
}
